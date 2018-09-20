Public Class frmNew

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        'Cancel operation
        sStr1 = ""
        cboCat.SelectedIndex = -1
        cboBy.SelectedIndex = -1
        Me.Close()
    End Sub

    Private Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.Click
        'Check all entries

        Dim strnNum As String = "000000"

        sNewDwgNo = Trim(lblDwgNo.Text)
        'sDeleted = ""


        If cboCat.Text = "" Then
            MsgBox("Select a category", MsgBoxStyle.Critical, "Missing Category")
            Exit Sub
        Else
            sNewCat = Trim(cboCat.SelectedItem)
        End If

        If cboBy.Text = "" Then
            MsgBox("Select a Name", MsgBoxStyle.Critical, "Missing Name")
            Exit Sub
        Else
            sNewName = Trim(cboBy.SelectedItem)
        End If

        If txtDescription.Text = "" Then
            MsgBox("Enter a description", MsgBoxStyle.Critical, "Missing Description")
            Exit Sub
        Else
            sNewDescription = Trim(txtDescription.Text)
        End If

        sNewJobNo = txtJobNo.Text

        myConn.Close()
        'Check if updating or creating a new one and acordingly ping database
        If bModify = False Then
            'Create new dwg no
            If sNewCat = "T7-System Drawings" Then
                If lblDwgNo.Text <> "" Then
                    'Split the string to check the format
                    sStr1 = lblDwgNo.Text
                    sStr2 = sStr1.Substring(0, 2)
                    If sStr2 <> "T7" Then
                        MsgBox("Please enter Dwg no in T7XXXXXX format", MsgBoxStyle.Critical, "Error !!")
                        Exit Sub
                    Else
                        'Check if no already exists
                        Try
                            myCmd.CommandText = "SELECT COUNT (DwgNum) FROM t_numbers where DwgNum='" & Trim(lblDwgNo.Text) & "'"
                            myConn.Open()
                            myReader = myCmd.ExecuteReader()
                            myReader.Read()
                            iInt1 = myReader.GetValue(0)
                            If iInt1 > 0 Then
                                'DwgNum already exists
                                MsgBox("Drawing No '" & lblDwgNo.Text & "' already exists.", MsgBoxStyle.Critical, "Error !!")
                                Exit Sub
                            End If
                        Catch ex As Exception
                            MsgBox("Error while adding into database " & ex.ToString, MsgBoxStyle.Critical, "Error !!")
                            Exit Sub
                        Finally
                            myReader.Close()
                            myConn.Close()

                        End Try

                    End If

                    sNewDwgNo = Trim(lblDwgNo.Text)
                Else
                    MsgBox("Please enter Dwg no in T7XXXXXX format", MsgBoxStyle.Critical, "Error !!")
                    Exit Sub
                End If

            End If

            Try
                If sNewCat <> "T7-System Drawings" Then
                    myCmd.CommandText = "SELECT MAX(CAST((SUBSTRING(DwgNum,2,7)) AS NUMERIC)) FROM t_numbers WHERE Category='" & sNewCat & "'"
                    myConn.Open()
                    myReader = myCmd.ExecuteReader()
                    myReader.Read()
                    iInt1 = myReader.GetValue(0)
                    myReader.Close()
                    myConn.Close()

                    strnNum = iInt1.ToString
                    'myConn.Dispose()

                    'Increment the number by one
                    iInt1 += 1


                    If sNewCat = "T0-Raw Material" Then
                        sNewDwgNo = "T" & Format(iInt1, "000000")
                    Else
                        sNewDwgNo = "T" & CStr(iInt1)
                    End If

                End If

                sNewDate = lblDate.Text

                sDeleted = ""
                myCmd.CommandText = "INSERT INTO t_numbers (DwgNum, Description, JobNum, CreateDate, Req, Category, Deleted) " &
                            "VALUES ('" & Trim(sNewDwgNo) & "','" & Trim(sNewDescription) & "','" & Trim(sNewJobNo) & "','" &
                            Trim(sNewDate) & "','" & Trim(sNewName) & "','" & Trim(sNewCat) & "','" & Trim(sDeleted) & "') "

                myConn.Open()
                myCmd.ExecuteNonQuery()
                myConn.Close()
                'myConn.Dispose()



                'Put in new dwg no in the textboxes
                frmMain.txtDwgNo.Text = sNewDwgNo
                frmMain.txtJobNo.Text = sNewJobNo
                frmMain.cboBy.Text = sNewName
                frmMain.cboCat.Text = sNewCat
                frmMain.txtSearch.Text = sNewDescription

                'Update grid
                frmMain.grdList.Rows.Add(New String() {"", sNewDwgNo, sNewDescription, sNewJobNo, lblDate.Text, sNewName, sNewCat})
                frmMain.grdList.ClearSelection()
                frmMain.grdList.Rows(frmMain.grdList.Rows.Count - 1).Selected = True
                frmMain.grdList.FirstDisplayedScrollingRowIndex = frmMain.grdList.RowCount - 1

                MsgBox("New Drawing Number '" & sNewDwgNo & "' successfully added !!", MsgBoxStyle.Information, "Success !!")

                Me.Close()
            Catch ex As Exception
                MsgBox("Error while adding into database " & ex.ToString, MsgBoxStyle.Critical, "Error !!")
                Exit Sub
            End Try
        Else
            'Modifying old dwg
            'Get date for modifying
            sNewDate = Now.Date
            Try
                myCmd.CommandText = "UPDATE t_numbers SET Description='" & Trim(sNewDescription) &
                                "', JobNum='" & Trim(sNewJobNo) & "', CreateDate='" & Trim(sNewDate) & "', Req='" & Trim(sNewName) &
                                "', Category='" & Trim(sNewCat) & "', Deleted='' WHERE DwgNum='" & Trim(sNewDwgNo) & "'"

                myConn.Open()
                myCmd.ExecuteNonQuery()
                myConn.Close()
                'myConn.Dispose()

                'Update textboxes
                frmMain.txtDwgNo.Text = sNewDwgNo
                frmMain.txtJobNo.Text = sNewJobNo
                frmMain.cboBy.Text = sNewName
                frmMain.cboCat.Text = sNewCat

                frmMain.txtSearch.Text = sNewDescription

                MsgBox("Drawing Number '" & sNewDwgNo & "' successfully modified !!", MsgBoxStyle.Information, "Success !!")
                lblDwgNo.Enabled = False
                cboCat.Enabled = True
                Me.Close()
            Catch ex As Exception
                MsgBox("Error while modifying the database " & ex.ToString, MsgBoxStyle.Critical, "Error !!")
                Exit Sub
            End Try
        End If


    End Sub

    Private Sub frmNew_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        'Show different forms if Creating or Modifying
        If bModify = False Then
            'New form
            Me.Text = "Create New Drawing No"
            'Me.BackColor = Color.LightGreen
            lblDate.BackColor = Color.LightGreen
            lblCreate.Text = "Create New Drawing Number"
            btnInsert.Text = "Accept"
            txtDescription.Text = ""
            lblDwgNo.Enabled = False
            cboCat.Enabled = True
            lblDwgNo.Text = ""
            txtJobNo.Text = ""
            lblModify.Visible = False
            lblDate.Text = Now.Date
            cboCat.Items.Clear()
            For iInt1 = 0 To sCatArr.Length - 2
                cboCat.Items.Add(sCatArr(iInt1))
            Next iInt1

            cboBy.Items.Clear()

            For iInt1 = 0 To sNameArr.Length - 1
                cboBy.Items.Add(sNameArr(iInt1))
            Next iInt1

        ElseIf bModify = True Then
            'Modifying form
            Me.Text = "Update Existing Drawing No"
            'Me.BackColor = Color.PaleTurquoise
            lblDate.BackColor = Color.PaleTurquoise
            btnInsert.Text = "Modify"
            lblDwgNo.Text = frmMain.grdList.Rows(frmMain.grdList.CurrentCell.RowIndex).Cells(1).Value
            txtDescription.Text = frmMain.grdList.Rows(frmMain.grdList.CurrentCell.RowIndex).Cells(2).Value
            txtJobNo.Text = frmMain.grdList.Rows(frmMain.grdList.CurrentCell.RowIndex).Cells(3).Value
            lblDate.Text = frmMain.grdList.Rows(frmMain.grdList.CurrentCell.RowIndex).Cells(4).Value

            lblCreate.Text = "Modifying Drawing " & lblDwgNo.Text

            cboCat.Items.Clear()
            For iInt1 = 0 To sCatArr.Length - 2
                cboCat.Items.Add(sCatArr(iInt1))
            Next iInt1

            cboBy.Items.Clear()
            lblModify.Visible = True

            For iInt1 = 0 To sNameArr.Length - 1
                cboBy.Items.Add(sNameArr(iInt1))
            Next iInt1

            cboBy.SelectedItem = frmMain.grdList.Rows(frmMain.grdList.CurrentCell.RowIndex).Cells(5).Value
            cboCat.SelectedItem = frmMain.grdList.Rows(frmMain.grdList.CurrentCell.RowIndex).Cells(6).Value
            cboCat.Enabled = False

        End If

    End Sub

    Private Sub txtJobNo_MouseClick(sender As Object, e As MouseEventArgs) Handles txtJobNo.MouseClick
        txtJobNo.SelectionStart = 0
    End Sub

    Private Sub cboCat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCat.SelectedIndexChanged
        sStr1 = cboCat.SelectedIndex
        If cboCat.SelectedIndex = 7 Then
            lblDwgNo.Enabled = True
            lblDwgNo.Text.ToUpper()
        Else
            lblDwgNo.Enabled = False
        End If
    End Sub
End Class