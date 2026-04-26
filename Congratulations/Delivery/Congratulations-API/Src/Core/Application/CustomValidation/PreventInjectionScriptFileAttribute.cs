using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Pdf.IO;

namespace _4EPlatform.Application.CustomValidation
{
    public class PreventInjectionScriptFileAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            // ✅ الحالة لو القيمة عبارة عن List<IFormFile>
            if (value is IEnumerable<IFormFile> files)
            {
                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                        continue;

                    var result = ValidateSingleFile(file);
                    if (result != ValidationResult.Success)
                        return result; // ترجع أول خطأ يحصل
                }
            }
            // ✅ الحالة لو القيمة عبارة عن ملف واحد
            else if (value is IFormFile singleFile)
            {
                var result = ValidateSingleFile(singleFile);
                if (result != ValidationResult.Success)
                    return result;
            }

            return ValidationResult.Success;
        }
        private ValidationResult ValidateSingleFile(IFormFile file)
        {

            var allowedExtensions = new[] { ".txt", ".pdf", ".docx" };
            var ext = System.IO.Path.GetExtension(file.FileName);
            if (allowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
            {
                string content = string.Empty;

                try
                {
                    if (ext.Equals(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        using var reader = new StreamReader(file.OpenReadStream());
                        content = reader.ReadToEnd();
                    }
                    else if (ext.Equals(".docx", StringComparison.OrdinalIgnoreCase))
                    {
                        using var memory = new MemoryStream();
                        file.CopyTo(memory);
                        memory.Position = 0;

                        using var doc = WordprocessingDocument.Open(memory, false);
                        content = doc.MainDocumentPart?.Document?.Body?.InnerText ?? string.Empty;
                    }
                    else if (ext.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        using var memory = new MemoryStream();
                        file.CopyTo(memory);
                        memory.Position = 0;

                        // ✅ اقرأ محتوى الملف كنص (للكشف عن سكربتات مخفية)
                        string pdfContent;
                        using (var reader = new StreamReader(new MemoryStream(memory.ToArray())))
                        {
                            pdfContent = reader.ReadToEnd();
                        }

                        // ✅ افحص وجود JavaScript داخل PDF
                        string[] pdfScriptIndicators = { "/JS", "/JavaScript", "/OpenAction" };
                        foreach (var indicator in pdfScriptIndicators)
                        {
                            if (Regex.IsMatch(pdfContent, indicator, RegexOptions.IgnoreCase))
                            {
                                return new ValidationResult($"🚫 الملف يحتوي على كود JavaScript غير آمن: {indicator}");
                            }
                        }

                        // ✅ استخدم PdfSharp للفحص النصي العادي (اختياري)
                        memory.Position = 0;
                        using var pdfDoc = PdfReader.Open(memory, PdfDocumentOpenMode.ReadOnly);
                        foreach (var page in pdfDoc.Pages)
                        {
                            content += page.Contents.ToString();
                        }
                    }

                    // ✅ تحقق من السكربتات الخبيثة  
                    string[] suspiciousPatterns =
                    {
             "<script", "javascript:", "onerror=", "onload=", "onclick=",
             "<iframe", "<object", "<embed", "<img", "<svg", "<link"
         };

                    foreach (var pattern in suspiciousPatterns)
                    {
                        if (Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase))
                        {
                            return new ValidationResult($"🚫 الملف يحتوي على كود غير آمن: {pattern}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new ValidationResult($"❌ خطأ أثناء فحص الملف: {ex.Message}");
                }
            }
            return ValidationResult.Success;
        }
    }

}
