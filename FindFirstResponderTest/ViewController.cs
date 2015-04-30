using System;
using Foundation;
using UIKit;

namespace FindFirstResponderTest
{
    public partial class ViewController : UIViewController
    {
        private NSObject _keyboardWillShowNotification;
        private NSObject _keyboardWillHideNotification;

        private UITextField _focusedField;
        private UIColor _originalColor;
        private UIView _tapView;

        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _tapView = new UIView();
            _tapView.AddGestureRecognizer(new UITapGestureRecognizer(() => View.EndEditing(force: true)));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _keyboardWillShowNotification = UIKeyboard.Notifications.ObserveWillShow((s, e) =>
                {
                    _focusedField = ResponderUtils.GetFirstResponder() as UITextField;
                    if (_focusedField != null)
                    {
                        _originalColor = _focusedField.BackgroundColor;
                        _focusedField.BackgroundColor = UIColor.LightGray;

                        View.AddSubview(_tapView);
                    }
                });

            _keyboardWillHideNotification = UIKeyboard.Notifications.ObserveWillHide((s, e) =>
                {
                    if (_focusedField != null)
                    {
                        _focusedField.BackgroundColor = _originalColor;
                        _focusedField = null;
                        _originalColor = null;

                        _tapView.RemoveFromSuperview();
                    }
                });
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardWillShowNotification);
            NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardWillHideNotification);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            _tapView.Frame = View.Bounds;
        }
    }
}

