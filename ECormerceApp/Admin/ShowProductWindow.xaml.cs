﻿using DataAccess.Repository.IRepository;
using DataObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ECormerceApp.Admin
{
    /// <summary>
    /// Interaction logic for ShowProductWindow.xaml
    /// </summary>
    public partial class ShowProductWindow : Window
    {
        enum OfContentType
        {
            Category,
            Supplier,
            Order
        }
        public int OfContent { get; set; }
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public int OrderID { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        public ShowProductWindow(IUnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
        }


        #region Method

        private void LoadFullData<T>(DataGrid dataGrid, IEnumerable<T> list)
        {
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = list;
        }

        private void ShowOnlyStackPanel(Border visibleBorder)
        {
            // Ẩn tất cả các StackPanel
            Border_ProductOfCategory.Visibility = Visibility.Collapsed;
            Border_ProductOfSupplier.Visibility = Visibility.Collapsed;
            // Hiển thị StackPanel mong muốn
            visibleBorder.Visibility = Visibility.Visible;
        }
        #endregion
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (OfContent)
            {
                case (int)OfContentType.Category:
                    ShowOnlyStackPanel(Border_ProductOfCategory);
                    var category = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryID == CategoryID);
                    var products = _unitOfWork.Product.GetAll(u => u.CategoryID == CategoryID, includeProperty: "Supplier");
                    TotalOfThisProduct.Text = "Total Product of " + category.CategoryName + ": " + products.Count();
                    LoadFullData<Product>(ProductOfCategoryDataGrid, products);
                    break;
                case (int)OfContentType.Supplier:
                    ShowOnlyStackPanel(Border_ProductOfSupplier);
                    var supplier = _unitOfWork.Supplier.GetFirstOrDefault(u => u.SupplierID == SupplierID);
                    var productsOfSupplier = _unitOfWork.Product.GetAll(u => u.SupplierID == SupplierID, includeProperty: "Category");
                    TotalOfThisProduct.Text = "Total Product of " + supplier.CompanyName + ": " + productsOfSupplier.Count();
                    LoadFullData<Product>(ProductOfSupplierDataGrid, productsOfSupplier);
                    break;
                case (int)OfContentType.Order:
                    ShowOnlyStackPanel(Border_ProductOfOrder);
                    var order = _unitOfWork.Order.GetFirstOrDefault(u => u.OrderID == OrderID);
                    var productsOfOrder = _unitOfWork.OrderDetail.GetAll(u => u.OrderID == OrderID, includeProperty: "Product");
                    TotalOfThisProduct.Text = "Total Product of Order " + order.OrderID + ": " + productsOfOrder.Count();
                    LoadFullData<OrderDetail>(ProductOfOrderDataGrid, productsOfOrder);
                    break;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }        
    }
}
