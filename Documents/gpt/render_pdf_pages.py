from pathlib import Path
import pypdfium2 as pdfium

src = Path(r"C:\GitLab\joboffer\Documents\gpt\rendered\timeline.pdf")
out = src.parent
pdf = pdfium.PdfDocument(src)
for i, page in enumerate(pdf):
    image = page.render(scale=1.6).to_pil()
    image.save(out / f"page-{i+1}.png")
print(len(pdf))
