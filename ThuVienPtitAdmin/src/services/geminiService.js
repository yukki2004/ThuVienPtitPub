export async function summarizeText(text) {
  const GEMINI_API_KEY = "AIzaSyCnilv8wjslOpWzwAKVOi5UbObVOBJLxlg";
  const MODEL = "gemini-2.5-flash";

  const res = await fetch(
    `https://generativelanguage.googleapis.com/v1beta/models/${MODEL}:generateContent?key=${GEMINI_API_KEY}`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        contents: [
          {
            parts: [
              {
                text: `Hãy tóm tắt nội dung sau ngắn gọn, rõ ràng, bằng tiếng Việt, chỉ lấy ý chính:\n\n${text}`,
              },
            ],
          },
        ],
      }),
    }
  );

  const data = await res.json();

  if (!res.ok) {
    console.error("❌ Lỗi Gemini API:", data);
    throw new Error(data?.error?.message || "Lỗi khi gọi Gemini API");
  }

  return data?.candidates?.[0]?.content?.parts?.[0]?.text || "Không thể tóm tắt.";
}
