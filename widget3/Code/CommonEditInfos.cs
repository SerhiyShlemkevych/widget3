using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using widget3.Controls.Concrete.SettingsWindow;

namespace widget3.Code
{
    public static class CommonEditPropertyInfos
    {
        public static TileEditPropertyInfo Background
        {
            get
            {
                DataTemplate itemTemplate = new DataTemplate();
                ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate();
                FrameworkElementFactory wrapPanel = new FrameworkElementFactory(typeof(WrapPanel));
                FrameworkElementFactory backgroundView = new FrameworkElementFactory(typeof(BackgroundTile));
                itemTemplate.VisualTree = backgroundView;
                itemsPanelTemplate.VisualTree = wrapPanel;
                ComboBox comboBox = new ComboBox()
                {
                    ItemTemplate = itemTemplate,
                    ItemsPanel = itemsPanelTemplate
                };
                comboBox.SetBinding(ComboBox.SelectedItemProperty, "SelectedTile.Background");
                comboBox.SetBinding(ComboBox.ItemsSourceProperty, "Backgrounds");
                comboBox.SetResourceReference(ComboBox.StyleProperty, "ComboBoxStyle");

                return new TileEditPropertyInfo()
                {
                    Label = "Background",
                    Control = comboBox
                };
            }
        }

        public static TileEditPropertyInfo Width
        {
            get
            {
                var comboBox = new ComboBox();
                comboBox.SetBinding(ComboBox.ItemsSourceProperty, "Configuration.TileWidths");
                comboBox.SetBinding(ComboBox.SelectedItemProperty, "SelectedTile.Width");
                comboBox.SetResourceReference(ComboBox.StyleProperty, "ComboBoxStyle");
                return new TileEditPropertyInfo()
                {
                    Label = "Width",
                    Control = comboBox
                };
            }
        }

        public static TileEditPropertyInfo Height
        {
            get
            {
                var comboBox = new ComboBox();
                comboBox.SetBinding(ComboBox.ItemsSourceProperty, "Configuration.TileHeights");
                comboBox.SetBinding(ComboBox.SelectedItemProperty, "SelectedTile.Height");
                comboBox.SetResourceReference(ComboBox.StyleProperty, "ComboBoxStyle");
                return new TileEditPropertyInfo()
                {
                    Label = "Height",
                    Control = comboBox
                };
            }
        }

        public static TileEditPropertyInfo Text
        {
            get
            {
                var textBox = new TextBox();
                textBox.SetBinding(ComboBox.ItemsSourceProperty, "Configutarion.TileHeights");
                textBox.SetBinding(TextBox.TextProperty, "SelectedTile.Data");
                textBox.SetResourceReference(TextBox.StyleProperty, "TextBoxStyle");
                return new TileEditPropertyInfo()
                {
                    Label = "Text",
                    Control = textBox
                };
            }
        }
    }
}
