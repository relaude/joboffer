import { FileBlob, SpreadsheetFile } from "@oai/artifact-tool";

const source = process.argv[2];
const input = await FileBlob.load(source);
const workbook = await SpreadsheetFile.importXlsx(input);

const sheets = await workbook.inspect({
  kind: "sheet",
  include: "id,name",
  maxChars: 4000,
});
console.log("SHEETS");
console.log(sheets.ndjson);

for (const sheetName of ["Responses", "Sheet1", "Sheet2"]) {
  const sheet = workbook.worksheets.getItem(sheetName);
  const used = sheet.getUsedRange(true);
  console.log(`DATA:${sheetName}`);
  console.log(JSON.stringify(used.values));
}
