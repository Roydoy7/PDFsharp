// PDFsharp - A .NET library for processing PDF
// See the LICENSE file in the solution root for more information.

using System;
using System.Diagnostics;
//using Microsoft.Extensions.Logging;
using PdfSharp.Fonts;
//using PdfSharp.Logging;

namespace PdfSharp.Snippets.Font
{
    /// <summary>
    /// This font resolver maps each request to a valid FontSource. If a typeface cannot be resolved by the PlatformFontResolver,
    /// it is substituted by a SegoeWP font.
    /// </summary>
    public class FailsafeFontResolver : IFontResolver
    {
        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            string typefaceName = $"'{familyName}' {(isBold ? "bold" : "")} {(isItalic ? "italic" : "")}";
            //var logger = LogHost.CreateLogger<FailsafeFontResolver>();

            // Ask platform first.
            var result = PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
            if (result != null)
            {
#if DEBUG
                Debug.WriteLine($"{typefaceName} resolved by PlatformFontResolver.");
#endif
                return result;
            }

            // Ask SegoeWpFontResolver.
            result = SegoeWpFontResolver.ResolveTypeface(familyName, isBold, isItalic);
            if (result != null)
            {
#if DEBUG
                Debug.WriteLine($"{typefaceName} resolved by SegoeWpFontResolver.");
#endif
                return result;
            }

            // Use SegoeWP or SegoeWPBold.
            result = SegoeWpFontResolver.ResolveTypeface(
                isBold ? SegoeWpFontResolver.FamilyNames.SegoeWPBold : SegoeWpFontResolver.FamilyNames.SegoeWP,
                false, isItalic);

            Debug.Assert(result != null);

#if DEBUG
            // No use of LoggerMessages here because this code is only for development.
            Debug.WriteLine($"{typefaceName} was substituted by a SegoeWP font.");
#endif

            return result;
        }

        /// <summary>
        /// Gets the bytes of a physical font with specified face name.
        /// </summary>
        /// <param name="faceName">A face name previously retrieved by ResolveTypeface.</param>
        /// <returns>
        /// The bytes of the font.
        /// </returns>
        public byte[]? GetFont(string faceName)
        {
#if DEBUG
            //var logger = LogHost.CreateLogger<FailsafeFontResolver>();
            Debug.WriteLine($"Get font for '{faceName}'.");
#endif

            // Note: PDFsharp never calls GetFont twice with the same face name.
            // Note: If a typeface is resolved by the PlatformFontResolver you never come here.
            return SegoeWpFontResolver.GetFont(faceName);
        }

        static readonly SegoeWpFontResolver SegoeWpFontResolver = new();
    }
}
