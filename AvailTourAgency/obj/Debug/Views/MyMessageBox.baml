   M S B A M L   `   `   `  ����J  FAvailTourAgency, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:AvailTourAgency.ViewsAvailTourAgency.Views  5   	   (    "AvailTourAgency.Views.MyMessageBox   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:AvailTourAgency.Views     5   	   b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          � Title$  MyMessageBox��5	   	   $	��300��6   $	��450��6+    � WindowStartupLocation$ CenterScreen=�67    � WindowStyle$
 None=�6\    � 
ResizeMode$ CanMinimize=�6o   $	��0.5q�6�   
���   �.��5
      � .+�5   
   � -   5         �Name$ Up��5      $��2��6   ���#4CB1F7 � RowDefinitions 5      �� 5      $��25 �6    6.   .+�5      � -   5        $��Toolbar��5      $��0��6"   ���	#FFFBFBFB$��0q�6F   $	��Top=�6Q   .+�5      � -   5         �Name$ Dragger��5      $��Center=�6+   
������� J� Opacity$	 0.2Q�6U   $	��370��6c   6o   �� 5       �� Orientation$ 
Horizontal=�6   $��Right=�66   .+�5      �� -   5        $��close_Button��5      ._�5      '  !MaterialDesignThemes.Wpf.PackIcon  	   Kind$	 Close=�62   $
��Auto��6?   $��19��6M   $��3,3,5,3q�6X   
������ ���#4CB1F7$��Stretch=�6�   5      5      5      5      �� 5   
   $��
25 30 25 5q�6   $	��400��6%   
�������$��Center=�6D   
 �� CornerRadius$
 10��6_   $��2��6r   $	��200��6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 30Q�6#      Color$ DarkSlateGray��63      ShadowDepth$ 0Q�6I   ._�5      �� .+�5      �� -   5         ��Name$
 Text��5      $��18(�6*   $��Regular"�68   $��Center=�6M   $��	0 30 0 10q�6j   ���#4CB1F7�� 6�   $P�JВы уверены что хотите закрыть программу?��6�   6�   �� 5      -   5         ��Name$ Text2��5      $�� ��6)    �� TextWrapping$
 Wrap=�61    �� AcceptsReturn .  �� 
IsReadOnly . $��Center=�6l   $��16(�6�   $��Regular"�6�   ���#4CB1F7$��10 10 10 20q�6�   6�   � 5      -   5        $ 	YesNoGrid��5      $��	60 0 60 0q�6*   .+�5       �� -	   5        $��	YesButton��5       $
��Да��6.   $
��Left=�6;   6V   �� 5!      -
   5        $��NoButton��5!      $��Нет��6-   $��Right=�6;   6W   5"      � 5#      -   5        $ OkGrid��5#      $��	60 0 60 0q�6'   $y�	Collapsed=�6:   .+�5$      �� -   5        $��OkButton��5$      $��OK��6-   $��Center=�6:   6W   5%      5&      5'      5(      5)      