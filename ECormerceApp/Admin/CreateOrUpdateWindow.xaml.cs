﻿using DataObject.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for CreateOrUpdateWindow.xaml
    /// </summary>
    public partial class CreateOrUpdateWindow : Window
    {
        private enum TypeWindow
        {
            Account,
        }
        private enum Type
        {
            Create,
            Update
        }
        public int TypeOfWindow;
        public int TypeOf;

        public Accounts updateAccount;
        private readonly UserManager<Accounts> _userManager;

        public CreateOrUpdateWindow(UserManager<Accounts> userManager)
        {
            InitializeComponent();
            _userManager = userManager;
        }

        private void ShowOnlyStackPanel(StackPanel stackPanel)
        {
            //Account
            CreateOrUpdateAccount.Visibility = Visibility.Collapsed;
            stackPanel.Visibility = Visibility.Visible;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(TypeOf == (int)Type.Create)
            {
                txtBanner.Text = "CREATE";
                switch (TypeOfWindow)
                {
                    case (int)TypeWindow.Account:
                        TitleOfCreateOrUpdate.Text = "ACCOUNT";
                        ShowOnlyStackPanel(CreateOrUpdateAccount);
                        break;
                }

            }
            else if (TypeOf == (int)Type.Update)
            {
                txtBanner.Text = "UPDATE";
                switch (TypeOfWindow)
                {
                    case (int)TypeWindow.Account:
                        TitleOfCreateOrUpdate.Text = "ACCOUNT";
                        ShowOnlyStackPanel(CreateOrUpdateAccount);
                        txtEmailInAccountContent.Text = updateAccount.Email;
                        txtFullNameInAccountContent.Text = updateAccount.FullName;
                        txtPhoneInAccountContent.Text = updateAccount.PhoneNumber;
                        if (updateAccount.Type == 1)
                        {
                            rdoStaffInAccountContent.IsChecked = true;
                        }
                        else
                        {
                            rdoNormalUserInAccountContent.IsChecked = true;
                        }
                        txtBlockPassConfirmInAccountContent.Visibility = Visibility.Collapsed;
                        txtBlockPassInAccountContent.Visibility = Visibility.Collapsed;
                        txtPassInAccountContent.Visibility = Visibility.Collapsed;
                        txtConfirmPassInAccountContent.Visibility = Visibility.Collapsed;
                        break;
                }
            }

            
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (TypeOf == (int)Type.Create)
            {
                switch (TypeOfWindow)
                {
                    case (int)TypeWindow.Account:
                        string email = txtEmailInAccountContent.Text;
                        string fullName = txtFullNameInAccountContent.Text;
                        string phone = txtPhoneInAccountContent.Text;
                        string pass = txtPassInAccountContent.Text;
                        string confirmPass = txtConfirmPassInAccountContent.Text;
                        bool isNormalUser = rdoNormalUserInAccountContent.IsChecked == true;
                        bool isStaff = rdoStaffInAccountContent.IsChecked == true;

                        //Check null
                        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(confirmPass))
                        {
                            MessageBox.Show("Please fill all fields");
                            return;
                        }
                        if (pass != confirmPass)
                        {
                            MessageBox.Show("Password and Confirm Password must be the same");
                            return;
                        }
                        if (isNormalUser == false && isStaff == false)
                        {
                            MessageBox.Show("Please choose type of account");
                            return;
                        }

                        var user = new Accounts
                        {
                            Email = email,
                            UserName = email,
                            FullName = fullName,
                            PhoneNumber = phone,
                            Type = isNormalUser ? 2 : 1
                        };
                        var result = await _userManager.CreateAsync(user, pass);
                        if (result.Succeeded)
                        {
                            if (user.Type == 1)
                            {
                                await _userManager.AddToRoleAsync(user, "Staff");

                            }
                            else
                            {
                                await _userManager.AddToRoleAsync(user, "NormalUser");
                            }
                        }
                        MessageBox.Show("Create account successfully");

                        foreach (var error in result.Errors)
                        {
                            MessageBox.Show(error.Description);
                        }
                        break;
                }

            }
            else if (TypeOf == (int)Type.Update)
            {
                switch (TypeOfWindow)
                {
                    case (int)TypeWindow.Account:
                        string email = txtEmailInAccountContent.Text;
                        string fullName = txtFullNameInAccountContent.Text;
                        string phone = txtPhoneInAccountContent.Text;
                        bool isNormalUser = rdoNormalUserInAccountContent.IsChecked == true;
                        bool isStaff = rdoStaffInAccountContent.IsChecked == true;
                        //Check null
                        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(phone))
                        {
                            MessageBox.Show("Please fill all fields");
                            return;
                        }
                        if (isNormalUser == false && isStaff == false)
                        {
                            MessageBox.Show("Please choose type of account");
                            return;
                        }
                        updateAccount.Email = email;
                        updateAccount.FullName = fullName;
                        updateAccount.PhoneNumber = phone;
                        updateAccount.Type = isNormalUser ? 2 : 1;
                        var result = await _userManager.UpdateAsync(updateAccount);
                        if (result.Succeeded)
                        {
                            if (updateAccount.Type == 1)
                            {
                                await _userManager.RemoveFromRoleAsync(updateAccount, "NormalUser");
                                await _userManager.AddToRoleAsync(updateAccount, "Staff");

                            }
                            else
                            {
                                await _userManager.RemoveFromRoleAsync(updateAccount, "Staff");
                                await _userManager.AddToRoleAsync(updateAccount, "NormalUser");
                            }
                        }
                        MessageBox.Show("Update account successfully");
                        break;
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
