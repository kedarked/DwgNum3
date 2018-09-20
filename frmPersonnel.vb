Public Class frmPersonnel
    Private Sub frmPersonnel_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.grdPersonnel.Font = New Font("Tahoma", 10, FontStyle.Regular)

        FillGridPersonnel()




    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        'Check if textboxes are filled up
        If txtFirst.Text = "" Then
            MsgBox("Enter First Name", MsgBoxStyle.Critical, "Error!")
            Exit Sub
        End If

        If txtLast.Text = "" Then
            MsgBox("Enter Last Name", MsgBoxStyle.Critical, "Error!")
            Exit Sub
        End If

        If txtInitials.Text = "" Then
            MsgBox("Enter Initials", MsgBoxStyle.Critical, "Error!")
            Exit Sub
        End If

        'Check if name is already there in the list
        For iInt1 As Integer = 0 To grdPersonnel.Rows.Count - 1
            If grdPersonnel.Rows(iInt1).Cells(0).Value = txtFirst.Text Then
                If grdPersonnel.Rows(iInt1).Cells(1).Value = txtLast.Text Then
                    MsgBox("Name already exists", MsgBoxStyle.Critical, "Error!")
                    Exit Sub
                End If
            End If
        Next iInt1

        'Name good now enter in database

        Try
            myCmd.CommandText = "INSERT INTO personnel (FirstName, LastName, FullName, Initials) " &
                            "VALUES ('" & Trim(txtFirst.Text) & "','" & Trim(txtLast.Text) & "','" & Trim(txtFirst.Text) &
                            " " & Trim(txtLast.Text) & "','" & Trim(txtInitials.Text) & "') "

            myConn.Open()
            myCmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox("Error while connecting to database-5" & ex.ToString, MsgBoxStyle.Critical, "Error !!")
            Exit Sub
        Finally
            myConn.Close()
        End Try


        'Refresh gridbox
        FillGridPersonnel()
        MsgBox(Trim(txtFirst.Text) & " " & Trim(txtLast.Text) & " added to the database", MsgBoxStyle.Information, "Success!!")

        txtFirst.Text = ""
        txtLast.Text = ""
        txtInitials.Text = ""


    End Sub

    Private Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        Me.Close()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtFirst.Text = ""
        txtLast.Text = ""
        txtInitials.Text = ""
    End Sub

    Sub FillGridPersonnel()

        grdPersonnel.Rows.Clear()

        Try
            myCmd.CommandText = "SELECT * FROM dbo.personnel ORDER By FullName ASC"
            myConn.Open()
            myReader = myCmd.ExecuteReader()

            iInt1 = 0
            Do While myReader.Read
                'Update Name List
                If Not IsDBNull(myReader.GetString(0)) Then sStr1 = myReader.GetString(0)
                If Not IsDBNull(myReader.GetString(1)) Then sStr2 = myReader.GetString(1)
                If Not IsDBNull(myReader.GetString(3)) Then sStr3 = myReader.GetString(3)

                grdPersonnel.Rows.Add(New String() {sStr1, sStr2, sStr3})

            Loop
        Catch ex As Exception
            MsgBox("Error while connecting to database-4" & ex.ToString, MsgBoxStyle.Critical, "Error !!")
            Exit Sub
        Finally
            myReader.Close()
            myConn.Close()
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If grdPersonnel.Rows.Count > 0 Then
            'Get current row
            sStr1 = grdPersonnel.Rows(grdPersonnel.CurrentCell.RowIndex).Cells(0).Value
            sStr2 = grdPersonnel.Rows(grdPersonnel.CurrentCell.RowIndex).Cells(1).Value

            sStr3 = MsgBox("Are you sure you want to delete " & sStr1 & " " & sStr2 & "?", vbYesNo + MsgBoxStyle.Question, "Confirm Delete")
            If sStr3 = vbYes Then
                Try

                    myCmd.CommandText = "DELETE FROM personnel WHERE FullName='" & sStr1 & " " & sStr2 & "'"

                    myConn.Open()
                    myCmd.ExecuteNonQuery()
                    MsgBox(sStr1 & " " & sStr2 & " deleted from database", MsgBoxStyle.Information, "Deleted !!")

                Catch ex As Exception
                    MsgBox("Error while deleting from database " & ex.ToString, MsgBoxStyle.Critical, "Error !!")
                    Exit Sub
                Finally
                    myConn.Close()
                End Try
            End If

            FillGridPersonnel()
        End If
    End Sub
End Class