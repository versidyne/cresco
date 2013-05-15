'Imports Microsoft.Win32

Public Class Registry

    ''' <summary>
    ''' Edits the value of a Registry Key. The key will be created if it does not already exist.
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <param name="Value"></param>
    ''' <param name="Path"></param>
    ''' <remarks></remarks>
    Public Sub Add(ByVal Name As String, ByVal Value As String, ByVal Path As String)

        Dim CU As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(Path)

        With CU

            .OpenSubKey(Path, True)

            .SetValue(Name, Value)

        End With

    End Sub

    ''' <summary>
    ''' Removes the key
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <param name="Path"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal Name As String, ByVal Path As String)

        Dim CU As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(Path)

        With CU

            .OpenSubKey(Path, True)

            .DeleteValue(Name, False)

            .DeleteSubKey(Name)

        End With

    End Sub

End Class
