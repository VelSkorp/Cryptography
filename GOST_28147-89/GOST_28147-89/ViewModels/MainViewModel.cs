﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GOST_28147_89
{
    public class MainViewModel : BaseViewModel
    {
        #region Public Properties

        public List<FileSystemObjectInfo> ItemsTextInput { get; set; }
        public List<FileSystemObjectInfo> ItemsTextOutput { get; set; }
        public List<FileSystemObjectInfo> ItemsKeyInput { get; set; }
        public FileSystemObjectInfo SelectedTextInputItem { get; set; }
        public FileSystemObjectInfo SelectedKeyInputItem { get; set; }
        public FileSystemObjectInfo SelectedTextOutputItem { get; set; }

        #endregion

        #region Commands

        public ICommand EncryptCommand { get; set; }

        public ICommand DecryptCommand { get; set; }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            ItemsTextInput = new List<FileSystemObjectInfo>();
            ItemsTextOutput = new List<FileSystemObjectInfo>();
            ItemsKeyInput = new List<FileSystemObjectInfo>();

            var drives = DriveInfo.GetDrives();
            DriveInfo.GetDrives().ToList().ForEach(drive =>
            {
                ItemsTextInput.Add(new FileSystemObjectInfo(drive));
                ItemsTextOutput.Add(new FileSystemObjectInfo(drive));
                ItemsKeyInput.Add(new FileSystemObjectInfo(drive));
            });

            EncryptCommand = new RelayCommand(Encrypt);
            DecryptCommand = new RelayCommand(Decrypt);
        }

        #endregion

        #region Command Methods

        public void Encrypt()
        {
            var file = new FileStream(SelectedTextInputItem.FileSystemInfo.FullName, FileMode.Open, FileAccess.Read);
            var reader = new StreamReader(file);
            string text = reader.ReadToEnd();
            reader.Close();

            file = new FileStream(SelectedKeyInputItem.FileSystemInfo.FullName, FileMode.Open, FileAccess.Read);
            reader = new StreamReader(file);
            string key = reader.ReadToEnd();
            reader.Close();

            var gOST_28147_89 = new GOST_28147_89Algorithm(text, key);
            text = gOST_28147_89.Encrypt();
            MessageBox.Show(text);

            file = new FileStream(SelectedTextOutputItem.FileSystemInfo.FullName, FileMode.Open, FileAccess.Write);
            var writer = new StreamWriter(file);
            writer.Write(text);
            writer.Close();
        }

        public void Decrypt()
        {
            var file = new FileStream(SelectedTextInputItem.FileSystemInfo.FullName, FileMode.Open, FileAccess.Read);
            var reader = new StreamReader(file);
            string text = reader.ReadToEnd();
            reader.Close();

            file = new FileStream(SelectedKeyInputItem.FileSystemInfo.FullName, FileMode.Open, FileAccess.Read);
            reader = new StreamReader(file);
            string key = reader.ReadToEnd();
            reader.Close();

            var gOST_28147_89 = new GOST_28147_89Algorithm(text, key);
            text = gOST_28147_89.Decrypt();
            MessageBox.Show(text);

            file = new FileStream(SelectedTextOutputItem.FileSystemInfo.FullName, FileMode.Open, FileAccess.Write);
            var writer = new StreamWriter(file);
            writer.Write(text);
            writer.Close();
        }

        #endregion
    }
}