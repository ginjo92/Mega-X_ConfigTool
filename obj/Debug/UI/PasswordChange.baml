   M S B A M L   `   `   `  ����K  GMegaXConfigTool, Version=1.1.2.13, Culture=neutral, PublicKeyToken=nullG(clr-namespace:MegaXConfigTool.PropertiesMegaXConfigTool.Properties  5   	   1clr-namespace:MegaXConfigToolMegaXConfigTool  5   	   U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5
   	   $    MegaXConfigTool.PasswordChange   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F BNHotkey.Wpf, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6p(clr-namespace:MegaXConfigTool.Properties     5   	   /localclr-namespace:MegaXConfigTool     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5
   	   -   5          � Title$  Mega-X Config Tool��5	   	   $	��400��6$   $	��300��61    � WindowState$ Normal=�5
   V    � WindowStartupLocation$ CenterScreen=�6k   $��Visible=�5       � 
ResizeMode$ NoResize=�6L   (  "MahApps.Metro.Controls.MetroWindow   ShowTitleBar .     IsWindowDraggable .    ShowMaxRestoreButton .   � WindowStyle$
 None=�5   6    � AllowsTransparency . $��1q�6c   
�������	 +� FocusedElement	 5   5   �� 
 �� ElementName$
 UserPasswordValue�� � 	Resources 5         UserLogins_dict S� 
MethodName S� 
ObjectType"  MegaXConfigTool.Dictionaries��%>   &	        S� 5   
   $ GetUserLogin��5   	   "  6#   5      ��5      �� 5   
    �� ImageSource$/ )/MegaXConfigTool;component/images/pcb.png��6      RightWindowCommands 5      +  %MahApps.Metro.Controls.WindowCommands  5   
   .@�5      �� -   5         ��Name$ TitleBarHelpButton��5      .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $(��"/images/topbaricons/help_small.png��6   $��20��6H   5      5      5      .��5!      � .+�5#   
   �� $��Center=�6   $��0q�60   $��Center=�6;   $	��450��6V   $	��320��6c   ���	#FFF7F7F7
���   � J� Effect 5$      3  -System.Windows.Media.Effects.DropShadowEffect  5%      ._�5'      �� 
�������.+�5(      !  MahApps.Metro.Controls.Tile  -   5          Name$ UserLoginHeader��5(   (   $
��Auto��6A   $	��100��6N      TitleFontSize$ 10Q�6[      HorizontalTitleAlignment$ Center=�5)       
���   �$	��Top=�6U   $��0,25,0,0q�6m   
���   �$��Center=�5*       $��Center=�6D   $
��Bold"�6f   $��20(�6x   {�.  .��5+      �� $ ��/images/login/changepw.png��6$   $	��180��6H   5,      �� 5-      .+�5.      �� *   $MegaXConfigTool.Properties.Resources   ChangePassword#��Z 6$   $��16(�6T   $
��Bold"�6b   $��Center=�6t   �� 50      $��Center=�6*   
�������$	��240��6_   $	��240��6k   $��10q�6x   .+�5?   "   �� -   5         ��Name$ UserPassword��5?   -   $ 
Horizontal=�6C   $��0,10,0,0q�6\   .+�5@   &     $��50��64   $
��Auto��6?   $ 10Q�6M   $ Stretch=�6`   $��0q�6�   $��Center=�6�   $��Center=�5A   (   $
��Bold"�6J   $��15(�6\   $
��Left=�6j      VerticalTitleAlignment$	 Top=�6�   $��1q�6�   
���   �{�.  ��5@   �   Z� .��5C   *   �� $ ��/images/login/password.png��60   $��40��6T   5D   '   �� 5E   &      CurrentPassword#��Z 6,   $��9(�6]   
�������$	��100��6|   G� 5F   &   -   5         G�Name$ UserPasswordValue��5F   2   $	��Top=�6c   $	��190��6{   $��50��6�   $
��Left=�6�   $��20(�6�   $��Center=�5G   .   $��Center=�6R   $��
-100,0,0,0q�6t   ��5F   M   Z� 5G   t   5H   #   �� 5J   "   -   5        $ UserNewPassword��5J   -   $ 
Horizontal=�6F   $��0,10,0,0q�6_   .+�5K   &     $��50��64   $
��Auto��6?   $ 10Q�6M   $ Stretch=�6`   
�������$��0q�6�   $��Center=�6�   $��Center=�5L   $   $
��Bold"�6F   $��15(�6X   $
��Left=�6f   $	 Top=�6�   ���	#FF705D5D$��1q�6�   
���   �{�.  .��5N   *   �� $#��/images/login/newpassword.png��60   $��45��6W   5O   '   �� 5P   &      NewPassword#��Z 6,   $��9(�6Y   
�������$	��100��6x   G� 5Q   &   -   5        $ UserNewPasswordValue��5Q   2   $	��Top=�6f   $	��190��6~   $��50��6�   $
��Left=�6�   $��20(�5R   .   $��Center=�6<   $��Center=�6`   $��
-100,0,0,0q�6�   ��5Q   P   Z� 5R   �   5S   #   �� 5U   "   -   5        $ UserRepeatNewPassword��5U   -   $ 
Horizontal=�6L   $��0,10,0,0q�6e   .+�5V   &     $��50��64   $
��Auto��6?   $ 10Q�6M   $ Stretch=�6`   
�������$��0q�6�   $��Center=�6�   $��Center=�5W   $   $
��Bold"�6F   $��15(�6X   $
��Left=�6f   $	 Top=�6�   ���	#FF705D5D$��1q�6�   
���   �{�.  .��5Y   *   �� $'��!/images/login/confirmpassword.png��60   $��45��6[   5Z   '   �� 5[   &      ConfirmNewPassword#��Z 6,   $��9(�6`   
�������$	��100��6   G� 5\   &   -	   5        $  UserRepeatNewPasswordValue��5\   2   $	��Top=�6l   $	��190��6�   $��50��6�   $
��Left=�6�   $��20(�5]   .   $��Center=�6<   $��Center=�6`   $��
-100,0,0,0q�6�   ��5\   V   Z� 5]   �   5^   #   �� 5`   "   -
   5        $ Buttons��5`   -   $ 
Horizontal=�6>   $��0,20,0,0q�6W   .+�5a   &     -   5        $ LoginOK_Tile��5a   4   $��90��6J   $��30��6U      ChangePasswordButton   Title# Z 6a   $ 10Q�6�   $��20,0,0,0q�6�   $	��Top=�5b   (   ���	#CC3DA254$ Right=�6W   
���   �.��5d   *   �� $ ��/images/login/login_ok.png��60   $��30,0,0,0q�6T   5e   '     5f   &   -   5        $ LoginCancel_Tile��5f   4   $��90��6N   $��30��6Y      Close# Z 6e   $ 10Q�6�   $��20,0,0,0q�6�   $	��Top=�5g   (   ���	#FFC94C46$ Right=�6r   
���   �.��5i   *   �� $$��/images/login/login_cancel.png��60   $��30,0,0,0q�6X   5j   '   5k   #   5m      5n      5o      5p      #  MahApps.Metro.Controls.Flyout  5r   
   -   5           Name$  
HelpFlyout��5r      $��Right=�5s      ���	#FF6E6A6A! J� Opacity$
! 0.75Q�5t      "   Help#��Z" 5u      $��0q�5v      #   Position$# Right=�5w      $   IsOpen$ .  $	��168��5x   )   $��Right=�65   $
��Bold"�6Q   .��5y      �� .+�5z      �� ���	#FFBBB7B7$��5,20,5,0q�65   % �� 
IsReadOnly% . . �5{      /� .T�5|      J� $��12(�6$   .*�62   .*�5}      �� &   Help_New_Password_H1_P1#�Z& 6"   $
��Bold"�6X   5~      J� 5      $��Italic$�6$   ' �� TextAlignment$
' Left=�67   $��10(�6L   ���	#FF8D3939.*�6q   .*�5�      �� (   Help_New_Password_H1_P2#�Z( 6"   5�      5�      5�      5�      5�      5�      5�      