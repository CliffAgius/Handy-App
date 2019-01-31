using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Xaml;
using System.Reflection;
using Xamarin.Forms;

namespace HandyApp
{
    /// <summary>
    /// DO NOT FORGET THAT YOU NEED TO MAKE THE IMAGE FILES "Embedded Resource Files" for this reflection system to find them...
    /// </summary>
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }



    }
}