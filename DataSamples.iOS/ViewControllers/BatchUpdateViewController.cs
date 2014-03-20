using DataSamples.Models;
using DataSamples.ViewModels;
using Intersoft.Crosslight;
using Intersoft.Crosslight.iOS;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace DataSamples.iOS.ViewControllers
{
    [Register("BatchUpdateViewController")]
    [RegisterNavigation("BatchUpdate")]
    [ImportBinding(typeof(EditableListBindingProvider))]
    public class BatchUpdateViewController : UITableViewController<EditableListViewModel>
    {
        #region Properties

        public override string CellIdentifier
        {
            get { return "TextboxTableCell"; }
        }

        public override TableViewCellStyle CellStyle
        {
            get { return TableViewCellStyle.Custom; }
        }

        public override UIViewTemplate CellTemplate
        {
            get { return new UIViewTemplate(TextboxTableCell.Nib); }
        }

        #endregion

        #region Methods

        protected override void InitializeView()
        {
            base.InitializeView();

            // Configure Toolbar
            UIBarButtonItem batchUpdateCommand = new UIBarButtonItem();
            batchUpdateCommand.Style = UIBarButtonItemStyle.Bordered;
            batchUpdateCommand.Title = "Batch Update";

            this.SetToolbarItems(new UIBarButtonItem[] {batchUpdateCommand}, false);
            this.RegisterViewIdentifier("BatchUpdateButton", batchUpdateCommand);

            this.RegisterViewIdentifier("textBox", batchUpdateCommand);

            // Show Toolbar
            this.NavigationController.SetToolbarHidden(false, true);
        }

        protected override void OnViewInitialized()
        {
            base.OnViewInitialized();

            // Use a custom table source with customized row height
            this.TableSource = new CustomTableSource(this, this.GetItemBinding(), this.ViewModel);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            // Hide Toolbar
            this.NavigationController.SetToolbarHidden(true, true);
        }

        #endregion
    }

    public class CustomTableSource : ObservableTableSource
    {
        #region Fields

        private EditableListViewModel _viewModel;
        private UITextFieldDelegate _textFieldDelegate;

        #endregion

        #region Properties

        private UITextFieldDelegate TextFieldDelegate
        {
            get
            {
                if (_textFieldDelegate == null)
                    _textFieldDelegate = new TextFieldDelegate(this, _viewModel);
                return _textFieldDelegate;
            }
        }

        #endregion

        #region Constructors

        internal CustomTableSource(UITableViewController viewController, ItemBindingDescription bindingDescription, EditableListViewModel viewModel) : base(viewController, bindingDescription, viewModel)
        {
            _viewModel = viewModel;
        }

        #endregion

        #region Methods

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            TextboxTableCell tableCell = base.GetCell(tableView, indexPath) as TextboxTableCell;
            tableCell.TextField.SetPropertyValue(TextFieldProperties.TableCellProperty, tableCell);

            if (tableCell.TextField.Delegate == null)
                tableCell.TextField.Delegate = TextFieldDelegate;
            return tableCell;
        }

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 75;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            base.RowSelected(tableView, indexPath);
            TextboxTableCell customCell = tableView.CellAt(indexPath) as TextboxTableCell;

            if (customCell != null)
                customCell.TextField.BecomeFirstResponder();
        }

        #endregion
    }

    public class TextFieldDelegate : UITextFieldDelegate
    {
        #region Fields

        private BindingContext _bindingContext;
        private readonly CustomTableSource _tableSource;
        private readonly EditableListViewModel _viewModel;

        #endregion

        #region Constructors

        public TextFieldDelegate(CustomTableSource tableSource, EditableListViewModel viewModel)
        {
            _tableSource = tableSource;
            _viewModel = viewModel;
        }

        #endregion

        #region Methods

        public override void EditingEnded(UITextField textField)
        {
            BindingManager.Unregister(_bindingContext);
        }

        public override void EditingStarted(UITextField textField)
        {
            // NOTE: Don't call the base implementation on a Model class
            // see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
            TextboxTableCell tableCell = textField.GetPropertyValue(TextFieldProperties.TableCellProperty) as TextboxTableCell;
            NSIndexPath index = _tableSource.ViewController.TableView.IndexPathForCell(tableCell);
            _tableSource.ViewController.TableView.SelectRow(index, false, UITableViewScrollPosition.None);

            Item item = _tableSource[_tableSource.ViewController.TableView.IndexPathForSelectedRow.Row] as Item;
            _bindingContext = textField.SetBinding(BindableProperties.TextProperty, new BindingDescription("Name", item, BindingMode.TwoWay));
            BindingManager.Register(_tableSource.ViewController as IViewContext, _bindingContext);
        }

        public override bool ShouldReturn(UITextField textField)
            {
            Item item = _tableSource[_tableSource.ViewController.TableView.IndexPathForSelectedRow.Row] as Item;
            textField.ResignFirstResponder();
            _viewModel.OnDataChanged(item);

            return true;
        }

        #endregion
    }

    public static class TextFieldProperties
    {
        #region Fields

        public static readonly BindableProperty TableCellProperty = BindableProperty.Register("TableCellProperty", typeof(UITableViewCell));

        #endregion
    }
}