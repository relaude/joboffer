import fs from "node:fs/promises";
import { FileBlob, SpreadsheetFile } from "@oai/artifact-tool";

const [source, output] = process.argv.slice(2);
const workbook = await SpreadsheetFile.importXlsx(await FileBlob.load(source));
const responseValues = workbook.worksheets.getItem("Responses").getUsedRange(true).values;
const mappingValues = workbook.worksheets.getItem("Sheet2").getUsedRange(true).values;

const headers = responseValues[0];
const rows = responseValues.slice(1);
const mappedColumns = mappingValues.slice(1).map((row) => row[1]);
if (headers.length !== mappedColumns.length + 1) {
  throw new Error(`Expected response Id plus ${mappedColumns.length} mapped columns; found ${headers.length} columns.`);
}

const dateColumns = new Set(["ResponseStartedAt", "ResponseCompletedAt", "FormCompletedDate"]);
const decimalColumns = new Set([
  "ExpectedMonthlyBasicSalary", "CurrentMonthlyBasicSalary", "AnnualGuaranteedBonusAmount",
  "MonthlyAllowanceAmount", "NonMonthlyAllowanceAmount", "MonthlyNonTaxableAllowanceAmount",
  "AnnualNonTaxableAllowanceAmount", "AnnualProfitSharingAmount", "AnnualIncentiveAmount",
  "AnnualVariablePayAmount",
]);

function excelDate(serial) {
  const millis = Math.round((serial - 25569) * 86400000);
  return new Date(millis).toISOString().replace("Z", "");
}

function sqlLiteral(value, column) {
  if (value === null || value === undefined || value === "") return "NULL";
  if (dateColumns.has(column)) {
    if (typeof value !== "number") throw new Error(`Non-numeric Excel date for ${column}: ${value}`);
    return `'${excelDate(value)}'`;
  }
  if (decimalColumns.has(column) || column === "CandidateResponseId") {
    if (typeof value !== "number") {
      const parsed = Number(String(value).replaceAll(",", ""));
      if (!Number.isFinite(parsed)) throw new Error(`Invalid numeric value for ${column}: ${value}`);
      return String(parsed);
    }
    return String(value);
  }
  return `N'${String(value).replaceAll("'", "''")}'`;
}

const columns = ["CandidateResponseId", ...mappedColumns];
const statements = rows.map((row) => {
  const values = [row[0], ...row.slice(1)].map((value, index) => sqlLiteral(value, columns[index]));
  return `INSERT INTO dbo.CandidateResponses (${columns.map((c) => `[${c}]`).join(", ")}) VALUES (${values.join(", ")});`;
});

const sql = [
  "SET NOCOUNT ON;",
  "SET XACT_ABORT ON;",
  "BEGIN TRANSACTION;",
  ...statements,
  "COMMIT TRANSACTION;",
  "SELECT COUNT(*) AS InsertedRows FROM dbo.CandidateResponses WHERE CandidateResponseId BETWEEN 1 AND 14;",
].join("\n");

await fs.writeFile(output, sql, "utf8");
console.log(JSON.stringify({ rows: rows.length, mappedColumns: mappedColumns.length, output }));
