Imports System.Data.SqlClient

Module Module1
    'Create ADO Objects
    Public myConn As SqlConnection
    Public myCmd As SqlCommand
    Public myReader As SqlDataReader
    Public results As String

    Public sDwgNo As String
    Public sItem As String
    Public sCategory As String
    Public sJobNo As String
    Public sName As String
    Public sStr1 As String
    Public sStr2 As String
    Public sStr3 As String
    Public sStr4 As String
    Public sStr5 As String
    Public sStr6 As String
    Public sStr7 As String
    Public sStr8 As String
    Public sDeleted As String

    Public sNewDwgNo As String
    Public sNewJobNo As String
    Public sNewCat As String
    Public sNewName As String
    Public sNewDate As String
    Public sNewDescription As String
    Public sCatArr(10) As String
    Public sNameArr(100) As String

    Public iInt1 As Integer
    Public iInt2 As Integer
    Public iInt3 As Integer
    Public iInt4 As Integer
    Public iInt5 As Integer

    Public bModify As Boolean

End Module
