Public Class VADP

    Private Key As String = "<|(cell)|>"
    Private SepKey As String = "<|(row)|>"
    Private Version As String = "0.1.0.0"

    Public Function Encode(ByVal ToString As String, ByVal FromString As String, ByVal Type As String, ByVal Data As String) As String

        Dim EncodedData As String = Key & Version & Key & ToString & Key & FromString & Key & Type & Key & Data & Key

        Return EncodedData

    End Function

    Public Function Decode(ByVal Code As String) As String()

        Dim DecodedData As String() = Nothing
        Dim Keys As String() = {Key}

        DecodedData = Code.Split(Keys, StringSplitOptions.None)

        Return DecodedData

    End Function

    Public Function MDConvert(ByVal Data As String) As Array

        'basic variables
        Dim Basic As Array
        Dim Row As String

        'complex variables
        Dim Dimensions As ArrayList = Nothing
        Dim Dimension As Array = Nothing

        'intial split to single dimensional array
        Basic = Split(Data, SepKey)

        'access each row
        For Each Row In Basic

            'create array out of row
            Dimension = Split(Row, Key)

            'add to main array
            Dimensions.Add(Dimension)

        Next

        Return Dimensions.ToArray

    End Function

    Public Function MDConvert(ByVal Dimensions As Array) As String

        'basic variables
        Dim Basic As String = Nothing

        'complex variables
        Dim Dimension As Array
        Dim Cell As String

        'access each dimension
        For Each Dimension In Dimensions

            Dim Row As String = Nothing

            'access each cell
            For Each Cell In Dimension

                'create string out of dimension
                Row += Key + Cell

            Next

            'add to main string
            Basic += Row

        Next

        Return Basic

    End Function

End Class
