/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:VoltAnalyzer.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight.Ioc;
using LanguageManager;
using Microsoft.Practices.ServiceLocation;
using VoltAnalyzer.Properties;
using VoltAnalyzer.Utils.FileDialog;
using VoltAnalyzer.ViewModel.PanelViewModels.Common;
using VoltAnalyzer.ViewModel.PanelViewModels.Home;
using VoltAnalyzer.Views;
using ViewModels.PanelViewModels.Common;
using VoltAnalyzer.Model.DataServices.Torque;
using VoltAnalyzer.Model.DataServices.SmoothDrive;
using System.IO;
using System;

namespace VoltAnalyzer.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator: ViewModelBase
    {
        #region ViewModelLocator Constructor
        public ViewModelLocator()
        {
            CurrentInstance = this;

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Texts.xml"))
            {
                ManageLanguage.loadXml(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Texts.xml"));
                ManageLanguage.ChangeLanguage("ENG");
            }

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<AbstractTorqueData, TorqueData>();

            SimpleIoc.Default.Register<HomePVM>();
            SimpleIoc.Default.Register<MessageDisplayVM>();
            SimpleIoc.Default.Register<FileDialogVM>();
        }
        #endregion

        #region Get current instance of ViewModelLocator
        public static ViewModelLocator CurrentInstance;
        #endregion

        #region instances

        public FileDialogVM FileDialogVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FileDialogVM>();
            }
        }

        public HomePVM HomeVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomePVM>();
            }
        }

        public AbstractTorqueData AbstractTorqueData
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AbstractTorqueData>();
            }
        }

        #endregion

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}