   M S B A M L   `   `   `  ����   VoltAnalyzer"    Views.Common.WaitChildWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � Title$  WaitChildWindow��   InvisibleBackground#��[   � WindowStyle$ 
ToolWindow=�.��� -    �Name$ 
LayoutRoot��$��Center=�$��Center=�$��91��$	��212�� � DataContext �� *WaitChildWindowVM+  ViewModelLocator �� Source# [  � RowDefinitions �� $
��Auto �.+��� $��Center=�$��Center=�$	��200��$
��Auto��$��0q� �� CornerRadius$ 20��$��91������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.��� 	 � Color$	 	#FFFDFDFB��
 � Offset$
 0Q�� $	 	#FFD6D7DC��$
 1Q�� $	 	#FFFBFBFB��$

 0.48Q�� $	 	#FFEDF0F5��$

 0.52Q�._�� $	��0,8q� �� $��0.583* ��� $��0.417* �.+��� $��25��$	��100��$
��Auto��$��Stretch=�$��Stretch=�$��Center=����� *	Message+� -    �Name$ progressBar1��$��26��$	��132�� � IsIndeterminate . $��Center=�$��Center=�$��1��