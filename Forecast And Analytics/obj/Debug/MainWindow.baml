   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5   	   Q MForecast And Analytics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Forecast_And_AnalyticsForecast_And_Analytics 5   	   '   !Forecast_And_Analytics.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	   6local$clr-namespace:Forecast_And_Analytics     5   	     � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)   .��5
      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0��6.   $��	0 10 0 10q�6;   .+�5      �� -   5        $��LoadCsvButton��5      $��📁 Load CSV��6*   $	��100��6@   6L   � 5      -   5        $��ProcessingBar��5      $	��200��6/   $��20��6;   $
��10 0q�6G   $y�	Collapsed=�6U   �� 5      -   5        $��EstimatedTimeText��5      $
��10 0q�61   $��Center=�6?   5      �� 5   
   $ 
Horizontal=�6   $��1��6.   $��	0 10 0 10q�6;   .+�5      �� $'��!Select Number of Days to Predict:��6   $��Center=�6A   $��0 0 10 0q�6\   �� 5      -   5        $��PredictionDaysComboBox��5      $	��100��65   .@�5      �� $��1��6    �� 
IsSelected . 6+   �� 5      $��3��6   �� 5      $��5��6   �� 5       $��7��6   5!      �� 5"      -   5        $��PredictButton��5"      $��Predict��6*   $	��100��6<   6H   5#      #   LiveCharts.Wpf.CartesianChart  5&   
   -   5        $��ForecastChart��5&      $��2��62   $	��300��6?   &    LiveCharts.Wpf.Charts.Base.Chart   LegendLocation$ Right=�6L   $��	0 10 0 10q�6c      AxisX 5(         LiveCharts.Wpf.Axis     Title$	 Day��6      AxisY 5+        $ Daily Yield (kWh)��6   5-      �� 50   
   -   5        $��ExplanationText��50      $��3��6+   $
��Wrap=�68   $��0 10 0 0q�6L   $��14(�6^   51      52      