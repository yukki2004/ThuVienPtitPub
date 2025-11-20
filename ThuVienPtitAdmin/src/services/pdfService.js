import * as pdfjsLib from "pdfjs-dist";
import { GlobalWorkerOptions } from "pdfjs-dist";
import pdfWorker from "pdfjs-dist/build/pdf.worker.min.mjs?url";

GlobalWorkerOptions.workerSrc = pdfWorker;
export async function extractTextFromPdf(url, numPagesToRead = 3) {
  try {
    const response = await fetch(url, { mode: "cors" });
    if (!response.ok) throw new Error("Không thể tải file PDF");

    const arrayBuffer = await response.arrayBuffer();
    const pdf = await pdfjsLib.getDocument({ data: arrayBuffer }).promise;

    const totalPages = Math.min(numPagesToRead, pdf.numPages);
    let text = "";

    for (let i = 1; i <= totalPages; i++) {
      const page = await pdf.getPage(i);
      const content = await page.getTextContent();
      const strings = content.items.map((item) => item.str).join(" ");
      text += `\n--- Trang ${i} ---\n${strings}\n`;
    }

    return text.trim();
  } catch (err) {
    console.error("❌ Lỗi khi đọc PDF:", err);
    throw err;
  }
}
