   M S B A M L   `   `   `  ����E  ACourseProj, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:CourseProj
CourseProj  5   	       CourseProj.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   *localclr-namespace:CourseProj     5   	   -   5          � Title$  Image detector v 0.2.6��5   	   $��555.581��6(   $��755.85��69   ���	#FF299942 � 	Resources 5         CourseProj.PSODBDataSet   pSODBDataSet  imgSetViewSource �� Source �� Source  resultViewSource  sessionViewSource��%�   &	        &	      7   &	 ?     7   &	 k     7     5   
   6   �� 5   
    68   �� *ImgSet+8   �� 5	   
    68   �� *Result+8   �� 5
   
    69   �� *	Session+8   5      .��5      �  � DataContext# [ 6   .+�5   
   o� $��	0,0,0,489q�6   .@�5      m� $��About��6   m� 5      $
��Help��6   m� 5      $
��Exit��6   5      �� 5   
   -   5        $��
Show image��5   #   $
��Left=�68   $��623,473,0,0q�6S   $	��Top=�6h   $	��115��6�   $��42��6�   �� 5   
   -   5         ��Name$ 
imageImage��5      $	��360��6$   $��375,41,10,0q�61   $	��Top=�6F   �� 5   
   -   5        $��Start processing��5   )   $
��Left=�6D   $��255,473,0,0q�6_   $	��Top=�6t   $	��115��6�   $��42��6�   �� 5   
   -   5        $��Choose image set��5   &   $
��Left=�6A   $��
10,473,0,0q�6\   $	��Top=�6p   $	��115��6�   $��42��6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ resultDataGrid��5         AutoGenerateColumns .     EnableRowVirtualization . $��10,41,378,358q�6�   	   RowDetailsVisibilityMode$	 VisibleWhenSelected=�6�   ��6f   �� # [ 
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      -   5        idColumn��,  &System.Windows.Controls.DataGridColumn   Header$ Id��5   N      Width5  /System.Windows.Controls.DataGridLengthConverter$ SizeToHeader 6Z   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *Id+6Z     5      -   5        
timeColumn��$
 Time��5   R   $ SizeToHeader 6`    69   �� *Time+6`     5      -	   5        accuracyColumn��$ Accuracy��5   Z   $ SizeToHeader 6l    6=   �� *
Accuracy+6l     5      -
   5        image_pathColumn��$ 
Image path��5   ^   $ SizeToHeader 6r    6?   �� *
Image path+6r     5      -   5        sessionIdColumn��$ 
Session Id��5   \   $ SizeToHeader 6p    6>   �� *	SessionId+6p   5        5   
   -   5        $ imgSetDataGrid��5       .   . $��10,172,378,223q�6~   $	 VisibleWhenSelected=�6�   ��6f   �� 
 5         5!      -   5        	idColumn1��$ Id��5!   O   $ SizeToHeader 6[    68   �� *Id+6[     5"      -   5        path_to_listColumn��$ Path to list��5"   b   $ SizeToHeader 6x    6A   �� *Path to list+6x     5#      -   5        number_of_imagesColumn��$ Number of images��5#   j   $ SizeToHeader 6�    6E   �� *Number of images+6�     5$      -   5        noise_ratingColumn��$ Noise rating��5$   b   $ SizeToHeader 6x    6A   �� *Noise rating+6x     5%      -   5        resolutionColumn��$ 
Resolution��5%   ^   $ SizeToHeader 6r    6?   �� *
Resolution+6r   5'        5(   
   -   5        $ sessionDataGrid��5(       .   . $��10,307,378,70q�6�   $	 VisibleWhenSelected=�6�   ��6g   �� # [ 
 5)        5*      -   5        	idColumn2��$ Id��5*   O   $ SizeToHeader 6[    68   �� *Id+6[     5+      -   5        wColumn��$ w��5+   L   $ SizeToHeader 6W    66   �� *w+6W     5,      -   5        c1Column��$	 c 1��5,   N   $ SizeToHeader 6[    67   �� *c1+6[     5-      -   5        c2Column��$	 c 2��5-   N   $ SizeToHeader 6[    67   �� *c2+6[     5.      -   5        tresholdColumn��$ treshold��5.   Z   $ SizeToHeader 6l    6=   �� *
treshold+6l     5/      -   5        imgSetIdColumn��$ 
Img Set Id��5/   Z   $ SizeToHeader 6n    6=   �� *
ImgSetId+6n   51      53      55      