Imports System.Diagnostics.FileVersionInfo
Public Class frmSplash

    Private Sub frmSplash_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        'Get version no
        'Dim fNewVersionInfo As FileVersionInfo
        Dim fCurrentVersion As FileVersionInfo
        'Dim sOpener As String


        ''Version.TryParse(Application.ProductVersion, CurrentVersion)
        fCurrentVersion = FileVersionInfo.GetVersionInfo(Application.ExecutablePath)
        lblVersion.Text = "Version: " & CStr(fCurrentVersion.FileVersion.ToString)


        'Tried Updater program. Too complicated when folder permissions come into picture on different laptops.. Maybe Future...
        'If System.Net.Dns.GetHostName = "W7SP1_VM" Then
        '    sOpener = "C:\Updates\DWgNum3\opener.txt"
        '    fNewVersionInfo = FileVersionInfo.GetVersionInfo("C:\Updates\DwgNum3\DwgNum3.exe")
        'Else
        '    sOpener = "C:\Program Files\Updater Program\opener.txt"
        '    fNewVersionInfo = FileVersionInfo.GetVersionInfo("S:\Engineering\Electrical Engineering\New Programs\Updates\DwgNum3\DwgNum3.exe")
        'End If

        'iInt1 = fNewVersionInfo.FileVersion.CompareTo(fCurrentVersion.FileVersion)

        'If iInt1 = 0 Then
        '    'Same version
        '    'Do nothing
        'ElseIf iInt1 = 1 Then
        '    'Later than.. 
        '    sStr2 = MsgBox("New version available. Update?", MsgBoxStyle.Information + vbYesNo, "Update Found")
        '    If sStr2 = vbYes Then
        '        If System.IO.File.Exists(sOpener) = True Then
        '            Dim obj As New System.IO.StreamWriter(sOpener, False)
        '            sStr1 = Application.ExecutablePath
        '            iInt2 = Application.ExecutablePath.LastIndexOf("\") + 1
        '            iInt3 = sStr1.Length
        '            sStr2 = sStr1.Substring(iInt2, iInt3 - iInt2)
        '            obj.WriteLine(sStr2)
        '            obj.Close()
        '        End If


        '        'Open Updater program
        '        Process.Start("C:\Program Files\Updater Program\Updater.exe")
        '        Me.Close()
        '        Application.Exit()


        '    End If
        'ElseIf iInt1 = -1 Then
        '    'Earlier that.. Do nothing.. Remove old file from the shared folder

        'End If
        tmrStartup.Enabled = True

    End Sub

    Private Sub tmrStartup_Tick(sender As Object, e As EventArgs) Handles tmrStartup.Tick

        'Start time count
        prgBar.Increment(10)
        If prgBar.Value = 100 Then
            frmMain.Show()
            tmrStartup.Enabled = False
            Me.Close()

        End If
    End Sub


End Class