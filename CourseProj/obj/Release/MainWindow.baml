   M S B A M L   `   `   `  ���� E  ACourseProj, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:CourseProj
CourseProj      CourseProj.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     *localclr-namespace:CourseProj     -     � Title$  Image detector v 0.2.6��$��555.581��$��755.85�����	#FF299942 � 	Resources    CourseProj.PSODBDataSet   pSODBDataSet  imgSetViewSource �� Source �� Source  resultViewSource  sessionViewSource��%�   &	        &	      7   &	 #     7   &	 A     7     ��  �� *ImgSet+8   ��  �� *Result+8   ��  �� *	Session+8   .���  � DataContext# [ .+�o� $��	0,0,0,489q�.@�m� -   $��About��m� -   $
��Help��m� -   $
��Exit���� -   $��
Show image��$
��Left=�$��623,473,0,0q�$	��Top=�$	��115��$��42���� -    ��Name$ 
imageImage��$	��360��$��375,41,10,0q�$	��Top=��� -    ��Name$ treshold��$
��Left=�$��400,473,0,0q�$	��Top=�$	��115��$��42���� -   $��Start processing��$
��Left=�$��255,473,0,0q�$	��Top=�$	��115��$��42���� -	   $��Choose image set��$
��Left=�$��
10,473,0,0q�$	��Top=�$	��115��$��42��&   System.Windows.Controls.DataGrid  -
     Name$ resultDataGrid��   AutoGenerateColumns .  	   EnableRowVirtualization	 . $��10,41,378,358q�
   RowDetailsVisibilityMode$
 VisibleWhenSelected=����� # [    Columns 0  *System.Windows.Controls.DataGridTextColumn  -   idColumn��,  &System.Windows.Controls.DataGridColumn   Header$ Id��   Width5  /System.Windows.Controls.DataGridLengthConverter$ SizeToHeader 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Id+  -   
timeColumn��$
 Time��$ SizeToHeader  �� *Time+  -   accuracyColumn��$ Accuracy��$ SizeToHeader  �� *
Accuracy+  -   image_pathColumn��$ 
Image path��$ SizeToHeader  �� *
Image path+  -   sessionIdColumn��$ 
Session Id��$ SizeToHeader  �� *	SessionId+  -   $ imgSetDataGrid�� .  	 . $��10,172,378,223q�$
 VisibleWhenSelected=�����    -   	idColumn1��$ Id��$ SizeToHeader  �� *Id+  -   path_to_listColumn��$ Path to list��$ SizeToHeader  �� *Path to list+  -   number_of_imagesColumn��$ Number of images��$ SizeToHeader  �� *Number of images+  -   noise_ratingColumn��$ Noise rating��$ SizeToHeader  �� *Noise rating+  -   resolutionColumn��$ 
Resolution��$ SizeToHeader  �� *
Resolution+  -   $ sessionDataGrid�� .  	 . $��10,307,378,70q�$
 VisibleWhenSelected=����� # [    -   	idColumn2��$ Id��$ SizeToHeader  �� *Id+  -   wColumn��$ w��$ SizeToHeader  �� *w+  -   c1Column��$	 c 1��$ SizeToHeader  �� *c1+  -   c2Column��$	 c 2��$ SizeToHeader  �� *c2+  -   tresholdColumn��$ treshold��$ SizeToHeader  �� *
treshold+  -   imgSetIdColumn��$ 
Img Set Id��$ SizeToHeader  �� *
ImgSetId+