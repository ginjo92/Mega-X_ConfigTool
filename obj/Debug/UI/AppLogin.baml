   M S B A M L   `   `   `  ����K  GMegaXConfigTool, Version=1.1.2.13, Culture=neutral, PublicKeyToken=nullG(clr-namespace:MegaXConfigTool.PropertiesMegaXConfigTool.Properties  5   	   1clr-namespace:MegaXConfigToolMegaXConfigTool  5   	   U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5
   	       MegaXConfigTool.AppLogin   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F BNHotkey.Wpf, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6p(clr-namespace:MegaXConfigTool.Properties     5   	   /localclr-namespace:MegaXConfigTool     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5
   	   -   5          � Title$  Mega-X Config Tool��5	   	   $	��300��6$   $	��300��61    � Icon$ /images/icon.ico��6=    � WindowState$ Normal=�5
   V    � WindowStartupLocation$ CenterScreen=�6k   $��Visible=�5      $	��640��6L   $	��480��6[   (  "MahApps.Metro.Controls.MetroWindow   ShowTitleBar .     IsWindowDraggable .    ShowMaxRestoreButton .   � WindowStyle$
 None=�5   �    � AllowsTransparency . 	 +� FocusedElement	 6P   �� 
 �� ElementName$
 UserPasswordValue�� � 	Resources 5         UserLogins_dict S� 
MethodName S� 
ObjectType"  MegaXConfigTool.Dictionaries��%>   &	        S� 5   
   $ GetUserLogin��5   	   "  6#   5      ��5      �� 5   
    �� ImageSource$/ )/MegaXConfigTool;component/images/pcb.png��6   .��5      � .+�5!   
   �� $��Center=�6   $��0q�60   $��Center=�6;   $	��350��6V   $	��330��6c   ���	#FFF7F7F7
���   � J� Effect 5"      3  -System.Windows.Media.Effects.DropShadowEffect  5#      ._�5%      �� 
�������.+�5&      !  MahApps.Metro.Controls.Tile  -   5          Name$ UserLoginHeader��5&       $
��Auto��69   $	��100��6F      TitleFontSize$ 10Q�6S      HorizontalTitleAlignment$ Center=�5'       $	��Top=�6B   $��0q�6Z   
���   �$��Center=�5(       $��Center=�6D   $
��Bold"�6f   $��20(�6x   {�. .��5)      �� $��/images/mega-x2.png��6   5*      �� 55      *   $MegaXConfigTool.Properties.Resources   Login#��Z 6   $��Center=�6?   $��18(�6\   $
��Bold"�6j   $��0,20,0,5q�6|   � 57      $
��Left=�6   $��	20,0,20,0q�64   $	��Top=�6G   
�������$	��330��6r   $��50��6~   .+�58        -   5        $ UserPassword��58   $   $��50��6:   $
��Auto��6E   $ 10Q�6S   $ Stretch=�6f   
�������$��0q�6�   $��Center=�59   $   $��Center=�6H   $
��Bold"�6j   $��15(�6|   $
��Left=�6�      VerticalTitleAlignment$	 Top=�5:   $   ���	#FF705D5D$��1q�6X   
�������{�. .��5;      �� $	��0,0q�6%   $��50��62   .+�5<      �� $ ��/images/login/password.png��6$   $��40��6H   $	��0,0q�6T    J� Opacity$
 0.75Q�6a   �� 5=      $��Password��6(   $��Center=�68   $��8(�6U   
�������$��20��6t   $
��0,-5q�6�   $��Center=�6�   5>      5?      G� 5@      -   5         G�Name$ UserPasswordValue��5@   "   
�������$	��Top=�6P   $	��240��6h   $��50��6t   $
��Left=�6�   
�������$��0,1,1,1q�5A   U   $��50,0,0,0q�6o   $��20(�6�   $��Center=�5B   =   $��Center=�6a   5C   =   5D      �� 5F       �� Orientation$ 
Horizontal=�6   $��Center=�66   $
��0,20q�6S   .+�5G        -   5        $ LoginOK_Tile��5G   $   $��90��6:   $��30��6E      LoginOk   Title# Z 6Q   $ 10Q�6x   $��0,0,0,0q�6�   $	��Top=�5H   (   ���	#CC3DA254$ Right=�6W   
���   �.��5J      �� $ ��/images/login/login_ok.png��6    $��30,0,0,0q�6D   5K        5L      -   5        $ LoginCancel_Tile��5L   $   $��90��6>   $��30��6I      Close# Z 6U   $ 10Q�6z   $��10,0,0,0q�6�   $	��Top=�5M   (   ���	#FFC94C46$ Right=�6r   
���   �.��5O      �� $$��/images/login/login_cancel.png��6    $��30,0,0,0q�6H   5P      5Q      �� 5S      .+�5T      �� $
��0,30q�6#   .@�5U      �� $ 
Horizontal=�6%   $��Right=�6>   $��5q�6Z   .+�5V      �� -   5         ��Name$ RadioLocalePT_Flag��5V   )   $ 
Horizontal=�6E   .+�5W   "   �� -   5         ��Name$ RadioLocalePT��5W   )   $��Center=�6@   $��2q�6]   .��5X   &   �� -	   5         ��Name$ FlagPT��5X   ,   $��25��6<   ��5Y   &   �� 5Z   *    �� 
TargetType" ��6:   .R�5[   .   �� #��z��6?   5\   +   5^   #   �� 5_   "   -
   5        $ RadioLocaleEN��5_   )   $��Center=�6@   $��2q�6]   .��5`   &   �� -   5        $ FlagEN��5`   ,   $��25��6<   ��5a   &   �� 5b   *   " ��6:   .R�5c   .   �� #��z��6?   5d   +   5f   #   5g      5h      5i      5j      5k      5l      5�      5�      