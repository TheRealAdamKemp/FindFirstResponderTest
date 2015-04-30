using Foundation;
using ObjCRuntime;
using UIKit;

namespace FindFirstResponderTest
{
    /// <summary>
    /// Utilities for UIResponder.
    /// </summary>
    public static class ResponderUtils
    {
        internal const string GetResponderSelectorName = "AK_findFirstResponder:";
        private static readonly Selector GetResponderSelector = new Selector(GetResponderSelectorName);

        /// <summary>
        /// Gets the current first responder if there is one.
        /// </summary>
        /// <returns>The first responder if one was found or null otherwise..</returns>
        public static UIResponder GetFirstResponder()
        {
            using (var result = new GetResponderResult())
            {
                UIApplication.SharedApplication.SendAction(GetResponderSelector, target: null, sender: result, forEvent: null);
                return result.FirstResponder;
            }
        }

        internal class GetResponderResult : NSObject
        {
            public UIResponder FirstResponder { get; set; }
        }
    }

    [Category(typeof(UIResponder))]
    internal static class FindFirstResponderCategory
    {
        [Export(ResponderUtils.GetResponderSelectorName)]
        public static void GetResponder(this UIResponder responder, ResponderUtils.GetResponderResult result)
        {
            result.FirstResponder = responder;
        }
    }
}

