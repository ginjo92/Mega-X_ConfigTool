   M S B A M L   `   `   `  ����K  GMegaXConfigTool, Version=1.1.2.13, Culture=neutral, PublicKeyToken=nullG(clr-namespace:MegaXConfigTool.PropertiesMegaXConfigTool.Properties  5   	   1clr-namespace:MegaXConfigToolMegaXConfigTool  5   	   U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5
   	        MegaXConfigTool.DataChoose   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F BNHotkey.Wpf, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6p(clr-namespace:MegaXConfigTool.Properties     5   	   /localclr-namespace:MegaXConfigTool     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5
   	     � Title$   ��5	   	   $	��300��6   $	��200��6    � WindowState$ Normal=�5
   V    � Topmost .  � WindowStartupLocation$ CenterScreen=�6z   $��Visible=�6�   $	��600��6�   $	��200��6�   (  "MahApps.Metro.Controls.MetroWindow   ShowTitleBar .    IsWindowDraggable .     ShowMaxRestoreButton .   � 
ResizeMode$ NoResize=�6>     IsMinButtonEnabled .   	   IsMaxRestoreButtonEnabled	 .  
   IsCloseButtonEnabled
 .  
���   � � 	Resources 5         UserLogins_dict S� 
MethodName S� 
ObjectType"  MegaXConfigTool.Dictionaries��%>   &	        S� 5   
   $ GetUserLogin��5   	   "  6#   5      .��5      � .+�5   
   �� $��0q�6   
�������
���   � J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      ._�5      �� .+�5      �� -   5         ��Name$ DataChooseDialog��5      $��15(�66   $
��Bold"�6D    �� TextAlignment$ Center=�6V   
���   �6m   �� 5       �� Orientation$ 
Horizontal=�6   $	��Top=�66   $
��0,20q�6N   .+�5      � $
��Left=�6   
�������$��	40,10,0,0q�6J   $	��160��6]    � RowDefinitions 5      �� 5      $��40 �6(   �� 5      $��40 �6(   �� 5      $��40 �6(   �� 5      $��40 �6(    � ColumnDefinitions 5!      �� 5"      $	��150 �6+   �� 5#      $	��150 �6+   �� 5$      $	��150 �6+   .+�5'      �� -   5         ��Name$ CheckBoxAreas��5'      $��0��66   $��0��6C   $��10,10q�6S    d� 	IsChecked$
 TrueY�6b   .��5(      �� .+�5)      �� *   $MegaXConfigTool.Properties.Resources   Areas#��Z 6(   5*      5+      �� 5,      -   5        $ CheckBoxZones��5,      $��0��66   $��1��6C   $��10,10q�6S   $
 TrueY�6b   .��5-      �� .+�5.      ��    Zones#��Z 6(   5/      50      �� 51      -   5        $ CheckBoxKeypads��51      $��0��68   $��2��6E   $��10,10q�6U   $
 TrueY�6d   .��52      �� .+�53      ��    Keypads#��Z 6(   54      55      �� 56      -   5        $ CheckBoxOutputs��56      $��1��68   $��0��6E   $��10,10q�6U   $
 TrueY�6d   .��57      �� .+�58      ��    Outputs#��Z 6(   59      5:      �� 5;      -   5        $ CheckBoxUsers��5;      $��1��66   $��1��6C   $��10,10q�6S   $
 TrueY�6b   .��5<      �� .+�5=      ��    Users#��Z 6(   5>      5?      �� 5@      -   5        $ CheckBoxTimezones��5@      $��1��6:   $��2��6G   $��10,10q�6W   $
 TrueY�6f   .��5A      �� .+�5B      ��    	Timezones#��Z 6(   5C      5D      �� 5E      -   5        $ CheckBoxPhones��5E      $��2��67   $��0��6D   $��10,10q�6T   $
 TrueY�6c   .��5F      �� .+�5G      ��    Phones#��Z 6(   5H      5I      �� 5J      -	   5        $ CheckBoxDialer��5J      $��2��67   $��1��6D   $��10,10q�6T   $
 TrueY�6c   .��5K      �� .+�5L      ��    Dialer#��Z 6(   5M      5N      �� 5O      -
   5        $ CheckBoxSystem��5O      $��2��67   $��2��6D   $��10,10q�6T   $
 TrueY�6c   .��5P      �� .+�5Q      ��    SystemConfiguration#��Z 6(   5R      5S      �� 5T      -   5        $&  CheckBoxAudioSystemConfiguration��5T      $��3��6I   $��0��6V   $��10,10q�6f   $
 TrueY�6u   .��5U      �� .+�5V      ��    Audio#��Z 6(   5W      5X      �� 5Y      -   5        $ CheckBoxExpanders��5Y   #   $��3��6>   $��1��6K   $��10,10q�6[   $
 TrueY�6j   .��5Z      �� .+�5[   "   ��     	Expansors#��Z  6,   5\      5]      5^      �� 5_      -   5        ! ��Name$! ButtonsAllNone��5_      $��Center=�65   $	��Top=�6R   $��20,10q�6j   .+�5`      !  MahApps.Metro.Controls.Tile  -   5        "  Name$" ButtonChooseAll��5`   $   $��50��6=   $��30��6H   #   Select_Everything$   Title#$ Z# 6T   %   TitleFontSize$% 10Q�6�   $	��Top=�5a   (   
����ح�&   HorizontalTitleAlignment$& Center=�6W   
���   �5b   (     5c      -   5        $" ButtonChooseNothing��5c   $   $��50��6A   $��30��6L   '   Select_Nothing#$ Z' 6X   $% 10Q�6�   $	��Top=�5d   (   
���z���$& Center=�6{   
���   �5e   (   5f      5g      ]� 5h      -   5        ( ]�Name$( Buttons��5h      $��60��6-   $	��260��69   $��Center=�6E   $��Bottom=�6b   $��-5q�6}   .+�5i        -   5        $" ButtonWrite��5i   $   $	��120��69   $��30��6E   )   Upload#$ Z) 6Q   $% 10Q�6w   $	��Top=�5j   (   
�������$& Right=�6X   
���   �$��0,0,0,0q�5k   H   $��1q�6Y   
�������$y�	Collapsed=�6�   .��5l      �� $!��/images/toptiles/upload.png��6    5m        5n      -   5        $" 
ButtonRead��5n   $   $	��120��68   $��30��6D   *   Download#$ Z* 6P   $% 10Q�6x   $	��Top=�5o   (   
�������$& Right=�6X   
���   �$��0,0,0,0q�5p   H   $��1q�6Y   
�������$y�	Collapsed=�6�   .��5q      �� $#��/images/toptiles/download.png��6    5r        5s      -   5        $" ButtonCancel��5s   $   $	��120��6:   $��30��6F   +   Button_cancel#$ Z+ 6R   $% 10Q�6   $	��Top=�5t   (   ���	#FFC94C46$& Right=�6r   
���   �$��10,0,0,0q�5u   ;   .��5v      �� $$��/images/login/login_cancel.png��6    $��0,0,0,0q�6H   5w      5x      5{      5|      5}      5~      