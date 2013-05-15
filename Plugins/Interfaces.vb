
''' <summary>
''' Interface host.
''' </summary>
''' <remarks></remarks>
Public Interface Host

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strFeedback"></param>
    ''' <remarks></remarks>
    Sub ShowFeedback(ByVal strFeedback As String)

End Interface

''' <summary>
''' Interface plugin.
''' </summary>
''' <remarks></remarks>
Public Interface Plugin

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Host"></param>
    ''' <remarks></remarks>
    Sub Initialize(ByVal Host As Host)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Name() As String

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Args">Arguments that will be passed to the plugin.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Main(ByVal Args As Array) As Integer

End Interface