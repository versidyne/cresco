
Public Class Startup

    'setvalue
    Public Sub Add(ByVal ApplicationName As String, ByVal ApplicationPath As String)
        Dim CU As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run")
        With CU
            .OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)

            .SetValue(ApplicationName, ApplicationPath)

        End With
    End Sub
    'remove value
    Public Sub Remove(ByVal ApplicationName As String)
        Dim CU As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run")
        With CU
            .OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)

            .DeleteValue(ApplicationName, False)

        End With
    End Sub

End Class
