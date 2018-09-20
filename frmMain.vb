Imports System.Data.SqlClient


Public Class frmMain

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        btnModify.Enabled = False
        btnModify.BackColor = Color.Transparent
        'Connection to SQL server
        'get computer name
        sStr1 = System.Net.Dns.GetHostName
        'sStr1 = "This"
        'If sStr1 = "W7SP1_VM" Then
        '    myConn = New SqlConnection("Server=W7SP1_VM\SQLEXPRESS; Database = thermaltech; Integrated Security=true;")
        'Else
        Try
            myConn = New SqlConnection("Server=TTL-FS1\THERMFS1SQL;Database=thermaltech;User Id=sa;Password=Therm_SQL")    ' use this finally
            'myConn = New SqlConnection("Server=RODNEYF5D6\SQLExpress;Database=thermaltech;User Id=sa;Password=Therm_SQL")   ' for in house testing
            myCmd = myConn.CreateCommand
            myCmd.CommandText = "SELECT Category FROM dbo.dwg_cat"
            myConn.Open()
            myReader = myCmd.ExecuteReader()
            sStr2 = myCmd.Connection.State
            myReader.Close()
            myConn.Close()
            'myConn.Dispose()
        Catch ex As Exception
            MsgBox("Error while connecting to database-1" & ex.ToString, MsgBoxStyle.Critical, "Error !!")
            Exit Sub
        End Try
        'End If

        'Fill up the combo boxes
        Try
            myCmd.CommandText = "SELECT Category FROM dbo.dwg_cat"
            'Open connection
            myConn.Open()

            myReader = myCmd.ExecuteReader()
            iInt1 = 0
            Do While myReader.Read
                If myReader.GetString(0) <> "" Then
                    'Update Category List
                    cboCat.Items.Add(myReader.GetString(0))
                    sCatArr(iInt1) = myReader.GetString(0)
                    iInt1 += 1
                End If
            Loop

            myReader.Close()
            myConn.Close()
            'myConn.Dispose()
        Catch ex As Exception
            MsgBox("Error while connecting to database-2" & ex.ToString, MsgBoxStyle.Critical, "Error !!")
            Exit Sub
        End Try

        'Check personnel
        Try
            myCmd.CommandText = "SELECT FullName FROM dbo.personnel ORDER By FullName ASC"
            myConn.Open()
            myReader = myCmd.ExecuteReader()

            iInt1 = 0
            Do While myReader.Read
                'Update Name List
                cboBy.Items.Add(myReader.GetString(0))
                sNameArr(iInt1) = myReader.GetString(0)
                iInt1 += 1
            Loop

            Array.Resize(sNameArr, iInt1)

            myReader.Close()
            myConn.Close()
            'myConn.Dispose()
        Catch ex As Exception
            MsgBox("Error while connecting to database-3" & ex.ToString, MsgBoxStyle.Critical, "Error !!")
            Exit Sub
        End Try

        'Update Status Bar
        stsStatusBar.Text = "No of Rows: "

        If grdList.Rows.Count = 0 Then
            btnDelete.Enabled = False
        End If

        Me.grdList.Font = New Font("Tahoma", 10, FontStyle.Regular)


    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        'Exit Program
        myConn.Close()
        'myConn.Dispose()
        Me.Close()
        Application.Exit()
    End Sub
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        'Clear all fields
        txtDwgNo.Text = ""
        txtSearch.Text = ""
        cboBy.Text = ""
        cboCat.Text = ""
        cboBy.SelectedIndex = -1
        cboCat.SelectedIndex = -1
        txtJobNo.Text = ""
        grdList.Rows.Clear()
        btnModify.Enabled = False
        btnModify.BackColor = Color.Transparent
        btnDelete.BackColor = Color.Transparent
        stsStatusBar.Text = "No of Rows: "
        If grdList.Rows.Count = 0 Then
            btnDelete.Enabled = False
        End If

    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        grdList.Rows.Clear()
        'Search for particular drawing or item
        Me.grdList.Font = New Font("Tahoma", 10, FontStyle.Regular)
        sCategory = cboCat.Text
        sName = cboBy.Text
        sDwgNo = txtDwgNo.Text
        sItem = txtSearch.Text

        If txtJobNo.Text = "    -" Then sJobNo = "" Else sJobNo = txtJobNo.Text

        'Search database
        Try
            myCmd.CommandText = "SELECT t_numbers.DwgNum, ecr_drawings.ECRNo, t_numbers.Description, " &
                                "t_numbers.JobNum, t_numbers.CreateDate, t_numbers.Req, t_numbers.Category, " &
                                "t_numbers.Deleted FROM t_numbers LEFT JOIN ecr_drawings ON " &
                                "t_numbers.DwgNum=ecr_drawings.DwgNo WHERE t_numbers.DwgNum Like ('%" &
                                sDwgNo & "%') AND t_numbers.Description Like ('%" & sItem & "%') AND " &
                                "t_numbers.JobNum Like ('%" & sJobNo & "%') AND t_numbers.Req Like ('%" &
                                sName & "%') AND " & "t_numbers.Category Like ('%" & cboCat.Text &
                                "%') ORDER BY t_numbers.DwgNum;"


            'myCmd.CommandText = "Select * FROM p_numbers WHERE DwgNum Like ('%" & sDwgNo & "%') AND " &
            '                    "Description Like ('%" & sItem & "%') AND " &
            '                    "JobNum Like ('%" & sJobNo & "%') AND " &
            '                    "Req Like ('%" & sName & "%') AND " &
            '                    "Category Like ('%" & cboCat.Text & "%') ORDER BY DwgNum;"

            myConn.Open()
            myReader = myCmd.ExecuteReader()
            iInt1 = 0
            Do While myReader.Read
                If IsDBNull(myReader.GetString(0)) Then sStr1 = "" Else sStr1 = myReader.GetString(0)       'DwgNum
                If IsDBNull(myReader.GetValue(1)) Then sStr2 = "" Else sStr2 = myReader.GetValue(1)         'ECRNo
                If IsDBNull(myReader.GetString(2)) Then sStr3 = "" Else sStr3 = myReader.GetString(2)       'Description
                If IsDBNull(myReader.GetValue(3)) Then sStr4 = "" Else sStr4 = myReader.GetValue(3)         'JobNum
                If IsDBNull(myReader.GetDateTime(4)) Then sStr5 = "" Else sStr5 = myReader.GetDateTime(4)   'CreateDate
                If IsDBNull(myReader.GetString(5)) Then sStr6 = "" Else sStr6 = myReader.GetString(5)       'Req
                If IsDBNull(myReader.GetString(6)) Then sStr7 = "" Else sStr7 = myReader.GetString(6)       'Category
                If IsDBNull(myReader.GetString(7)) Then sStr8 = "" Else sStr8 = myReader.GetString(7)       'Deleted

                'Update grid
                grdList.Rows.Add(New String() {sStr2, sStr1, sStr3, sStr4, sStr5, sStr6, sStr7})

                'Check if dwg no has been deleted or not
                If sStr8 = "Y" Then
                    grdList.Rows(grdList.Rows.Count - 1).DefaultCellStyle.Font = New Font(grdList.Font, FontStyle.Strikeout)
                Else
                    grdList.Rows(grdList.Rows.Count - 1).DefaultCellStyle.Font = New Font(grdList.Font, FontStyle.Regular)
                End If

                iInt1 += 1
            Loop

            'Update status bar
            stsStatusBar.Text = "No of Rows: " & iInt1

            myReader.Close()
            myConn.Close()
            'myConn.Dispose()
        Catch ex As Exception
            MsgBox("Error while connecting to database-3 " & ex.ToString, MsgBoxStyle.Critical, "Error !!")
            Exit Sub
        End Try


        'No results found
        If grdList.RowCount = 0 Then
            MsgBox("No Drawings Found !!", MsgBoxStyle.Critical, "No Results !!")
            btnModify.Enabled = False
            btnDelete.Enabled = False
            btnDelete.BackColor = Color.Transparent
            btnModify.BackColor = Color.Transparent
        Else
            btnModify.Enabled = True
            btnDelete.Enabled = True
            btnDelete.BackColor = Color.LightCoral
            btnModify.BackColor = Color.Turquoise
        End If



    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        'Generate new drawing No
        bModify = False
        'Call frmNew.ShowDialog()
        If frmNew.ShowDialog() <> DialogResult.Cancel Then
            Call btnSearch.PerformClick()
            If grdList.RowCount = 0 Then
                'MsgBox("No Drawings Found !!", MsgBoxStyle.Critical, "No Results !!")
                btnModify.Enabled = False
                btnDelete.Enabled = False
                btnDelete.BackColor = Color.Transparent
                btnModify.BackColor = Color.Transparent
            Else
                btnModify.Enabled = True
                btnDelete.Enabled = True
                btnDelete.BackColor = Color.LightCoral
                btnModify.BackColor = Color.Turquoise
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        'Delete row
        If grdList.Rows.Count > 0 Then
            'Get current row
            sStr1 = grdList.Rows(grdList.CurrentCell.RowIndex).Cells(1).Value
            sStr1 = MsgBox("Are you sure you want to delete the Dwg no '" & sStr1 & "'?", vbYesNo + MsgBoxStyle.Question, "Confirm Delete")
            If sStr1 = vbYes Then
                sStr2 = grdList.Rows(grdList.CurrentCell.RowIndex).Cells(1).Value
                Try
                    myCmd.CommandText = "UPDATE t_numbers SET Deleted='Y' WHERE DwgNum='" & Trim(sStr2) & "'"

                    myConn.Open()
                    myCmd.ExecuteNonQuery()
                    myConn.Close()
                    'myConn.Dispose()

                    'Remove from grid
                    grdList.Rows.Remove(grdList.CurrentRow)

                    'Clear all
                    txtDwgNo.Text = ""
                    txtJobNo.Text = ""
                    txtSearch.Text = ""
                    cboBy.Text = ""
                    cboCat.Text = ""
                    cboBy.SelectedIndex = -1
                    cboCat.SelectedIndex = -1
                    MsgBox("Drawing Number '" & sStr2 & "' successfully deleted !!", MsgBoxStyle.Information, "Deleted !!")
                    If grdList.Rows.Count = 0 Then
                        btnModify.Enabled = False
                        btnDelete.Enabled = False
                        btnDelete.BackColor = Color.Transparent
                        btnModify.BackColor = Color.Transparent
                    Else
                        btnModify.Enabled = True
                        btnDelete.Enabled = True
                        btnDelete.BackColor = Color.LightCoral
                        btnModify.BackColor = Color.Turquoise
                    End If
                Catch ex As Exception
                    MsgBox("Error while deleting from database " & ex.ToString, MsgBoxStyle.Critical, "Error !!")
                    Exit Sub
                End Try
            End If
        End If

    End Sub

    Private Sub btnModify_Click(sender As Object, e As EventArgs) Handles btnModify.Click
        'Update existing drawing
        If grdList.Rows.Count > 0 Then
            sStr1 = grdList.Rows(grdList.CurrentCell.RowIndex).Cells(1).Value

            sStr2 = MsgBox("Modify Drawing No '" & sStr1 & "' ?", vbYesNo + MsgBoxStyle.Question, "Modify ?")
            If sStr2 = vbYes Then
                bModify = True
                Call frmNew.ShowDialog()
                'frmNew.ShowDialog() <> DialogResult.Cancel Then
                Call btnSearch.PerformClick()
                'End If
            Else
                bModify = False
            End If
        End If
    End Sub

    Private Sub grdList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles grdList.CellClick
        Dim _sDwgNo As String
        Dim _sTitle As String
        Dim _sJobNo As String
        Dim _sBy As String
        Dim _sCategory As String

        'Update textboxes
        _sDwgNo = grdList.Rows(grdList.CurrentCell.RowIndex).Cells(1).Value
        _sTitle = grdList.Rows(grdList.CurrentCell.RowIndex).Cells(2).Value
        _sJobNo = grdList.Rows(grdList.CurrentCell.RowIndex).Cells(3).Value
        _sBy = grdList.Rows(grdList.CurrentCell.RowIndex).Cells(5).Value
        _sCategory = grdList.Rows(grdList.CurrentCell.RowIndex).Cells(6).Value

        txtDwgNo.Text = _sDwgNo
        txtSearch.Text = _sTitle
        txtJobNo.Text = _sJobNo
        cboBy.Text = _sBy
        cboCat.Text = _sCategory

        If grdList.Rows.Count > 0 Then
            btnModify.Enabled = True
            btnDelete.Enabled = True

            btnDelete.BackColor = Color.LightCoral
            btnModify.BackColor = Color.Turquoise

        End If
    End Sub

    Private Sub txtJobNo_MouseClick(sender As Object, e As MouseEventArgs) Handles txtJobNo.MouseClick
        'Send the cursor to starting position
        txtJobNo.SelectionStart = 0
    End Sub

    'Clear individual textboxes
    Private Sub btnClrDwg_Click(sender As Object, e As EventArgs) Handles btnClrDwg.Click
        txtDwgNo.Text = ""
    End Sub

    Private Sub btnClrCat_Click(sender As Object, e As EventArgs) Handles btnClrCat.Click
        cboCat.SelectedIndex = -1
    End Sub

    Private Sub btnClrBy_Click(sender As Object, e As EventArgs) Handles btnClrBy.Click
        cboBy.SelectedIndex = -1
    End Sub

    Private Sub btnClrDesc_Click(sender As Object, e As EventArgs) Handles btnClrDesc.Click
        txtSearch.Text = ""
    End Sub

    Private Sub btnClrJob_Click(sender As Object, e As EventArgs) Handles btnClrJob.Click
        txtJobNo.Text = ""
    End Sub

    Private Sub lblBy_Click(sender As Object, e As EventArgs)

        frmPersonnel.ShowDialog()

    End Sub

    Private Sub lblBy_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblBy.LinkClicked


        frmPersonnel.ShowDialog()

    End Sub
End Class