   M S B A M L   `   `   `  ����J  FAvailTourAgency, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:AvailTourAgency.ViewsAvailTourAgency.Views  5   	   3    -AvailTourAgency.Views.TourDatesPriceAddWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   X TXceed.Wpf.Toolkit, Version=4.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   /local#clr-namespace:AvailTourAgency.Views  5   	     � Title$  TourDatesPriceAddWindow��5
   	   $	��300��6)   $	��780��66    � WindowStyle$
 None=�6B    � 
ResizeMode$ CanMinimize=�6U    � WindowStartupLocation$ CenterScreen=�6n   $	��0.5q�6�   ���	#FF1796D7.��5      � .+�5   
   � -   5         �Name$ Up��5      $��2��6   
������� � RowDefinitions 5      �� 5      $��25 �6    6.   .+�5      � -   5        $��Toolbar��5      $��0��6"   ���	#FFFBFBFB$��0q�6F   $	��Top=�6Q   .+�5      � -   5         �Name$ Dragger��5      $��Center=�6+   
������� J� Opacity$	 0.2Q�6U   $	��370��6c   6o   �� 5       �� Orientation$ 
Horizontal=�6   $��Right=�66   .+�5      �� -   5        $��close_Button��5      ._�5      '  !MaterialDesignThemes.Wpf.PackIcon  	   Kind$	 Close=�62   $
��Auto��6?   $��19��6M   $��3,3,5,3q�6X   
������ ���#4CB1F7$��Stretch=�6�   5      5      5      5      � 5   
   .+�5      � -   5        $ ViewTourGrid��5      $y�Visible=�6)   $��
0,20,0.2,0q�6>   $	��103��6R   $	��Top=�6_   .+�5      �� $
��Left=�6   $	��773��68   $	��Top=�6D   $��
0,0,0,-189q�6\   .+�5      � .+�5      �� $��Дата вылета:��6$   $��16(�68   $��SemiBold"�6F   ���	#FF3580BF$��5 20 0 5q�6s   $��Center=�6�   6�   (  "System.Windows.Controls.DatePicker  5      -   5        $��FirstDatePicker��5   %    (  $MaterialDesignFloatingHintDatePicker#��[  6<   $��	109,0,0,0q�5   %   $��46��68   
 � Language$
 ru�6D   $��14(�6R   4  .System.Windows.Automation.AutomationProperties   IsOffscreenBehavior$ Onscreen=�5    %   $
��Left=�5!   %   $	��200��6@   ���	#FF3580BF$	��Top=�5"   %   5#      �� 5$      $+��%Дата вылета обратно:��6$   $��16(�6@   $��SemiBold"�6N   ���	#FF3580BF$��346,20,-0.2,4.4q�6{   $��Center=�6�   6�     5%      -   5        $��SecondDatePicker��5%   %   #��[  6=   $��	528,0,0,0q�5&   %   $��46��68   $
 ru�6D   $��14(�6R   $ Onscreen=�5'   %   $
��Left=�5(   %   $	��200��6@   ���	#FF3580BF$	��Top=�5)   %   5*      5+      � 5,      .+�5-      �� $��	Цена:��6$   $��16(�61   $��SemiBold"�6?   ���	#FF3580BF$��5 20 0 5q�6l   $��Center=�6~   6�   �� 5.      -   5        $��PriceBox��5.   "     MaterialDesignTextBox#��[ 62   $
 ru�5/   )   $��14(�67   $ Onscreen=�50   )   $
��Left=�51   )   $	��200��6D   ���	#FF3580BF$	��Top=�52   )   $��
107,10,0,0q�6A   $��31��6U   .��53   y   54      �� 55      $��Авиакомпания:��6$   $��16(�69   $��SemiBold"�6G   ���	#FF3580BF$��346,20,-0.2,4.4q�6t   $��Center=�6�   6�   �� 56      -	   5        $��
AirlineBox��56   "   #��[ 64   $
 ru�57   )   $��14(�67   $ Onscreen=�58   )   $
��Left=�59   )   $	��200��6D   ���	#FF3580BF$	��Top=�5:   )   $��
527,11,0,0q�6A   $��31��6U   .��6�   5;      5<      � 5=      .+�5>      �� $��Самолет:��6$   $��16(�64   $��SemiBold"�6B   ���	#FF3580BF$��5 20 0 5q�6o   $��Center=�6�   6�   �� 5?      -
   5        $��PlaneBox��5?   "   #��[ 62   $
 ru�5@   )   $��14(�67   $ Onscreen=�5A   )   $
��Left=�5B   )   $	��200��6D   ���	#FF3580BF$	��Top=�5C   )   $��
110,10,0,0q�6A   $��33��6U   .��5D   L   5E      �� 5F      $"��Класс перелета:��6$   $��16(�6;   $��SemiBold"�6I   ���	#FF3580BF$��346,20,-0.2,4.4q�6v   $��Center=�6�   6�   �� 5N      -   5        $��FlightClassBox��5N   #     MaterialDesignComboBox#��[ 69   ���	#FF3580BF$	��Top=�5O   %   $��527,10,46,0q�6=   $��14(�6R   $��33��6`   ��6l   �� *FlightClasses.Values+5P   %   5Q      � 5R      .+�5S      �� $��Багаж:��6$   $��16(�62   $��SemiBold"�6@   ���	#FF3580BF$��5 20 0 5q�6m   $��Center=�6   6�   �� 5T      -   5        $��
LuggageBox��5T   "   #��[ 64   $
 ru�5U   )   $��14(�67   $ Onscreen=�5V   )   $
��Left=�5W   )   $	��200��6D   ���	#FF3580BF$	��Top=�5X   )   $��
110,10,0,0q�6A   $��33��6U   .��5Y   N   5Z      �� 5[      $��Питание:��6$   $��16(�64   $��SemiBold"�6B   ���	#FF3580BF$��346,20,-0.2,4.4q�6o   $��Center=�6�   6�   �� 5\      -   5        $��MealsBox��5\   "   #��[ 62   $
 ru�5]   )   $��14(�67   $ Onscreen=�5^   )   $
��Left=�5_   )   $	��200��6D   ���	#FF3580BF$	��Top=�5`   )   $��
527,11,0,0q�6A   $��31��6U   .��5a   L   5b      5c      � 5d      $��0 10 0 0q�6   .+�5e      �� $��Количество:��6$   $��16(�67   $��SemiBold"�6E   ���	#FF3580BF$��5,14,645.8,9.8q�6r   $��Center=�6�   6�   %  Xceed.Wpf.Toolkit.IntegerUpDown  5f      -   5        $��QuantityBox��5f   -   $��14(�6@   $��114,10,558.8,-0.8q�6N   ���	#FF3580BF�  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Minimum$ 1X�6�      Maximum$	 100X�6�   6�   �� 5g      -   5         ��Name$ SaveTourDatesPrice��5g   !   $��Center=�6=     MaterialDesignFlatButton#��[ 6Z   $��Сохранить��6�   $	��376��6�   ���	#FF0017FF$��32��6�    �� 	ClickMode$ Release=�6�   $��348,13,48.8,-0.2q�6�   5h   !   5i      � 5j      $��5q�6   5k      5l      5m      5n      5o      5p      