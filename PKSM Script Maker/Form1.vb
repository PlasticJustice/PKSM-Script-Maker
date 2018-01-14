Public Class Form1
    Dim pathexe = My.Application.Info.DirectoryPath
    Dim path = pathexe & "\assets"
    Dim web As New System.Net.WebClient
    Dim prog As String = pathexe & "\PKSMScript.py"
    Dim sm As String
    Dim data(3) As String

    Dim stat As Integer = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim Everest_Registry As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\Python\PythonCore")
        If Everest_Registry Is Nothing Then
            'key does not exist
            Dim py As Integer = MsgB("Python 3+ is required for script creation. Would you like to get python or continue without script creation?", 2, "Get Python", "Continue Anyway", "", "Python Required")
            If py = 6 Then
                Dim webAddress As String = "https://www.python.org/downloads/"
                Process.Start(webAddress)
                Application.Exit()
            ElseIf py = 7 Then
                TabPage1.Enabled = False
            End If
        Else
        End If

        TabControl1.TabIndex += 1
        RichTextBox1.Hide()
        buttons()
        TabControl1.TabIndex -= 1
    End Sub
    Private Function MsgB(ByVal mes As String, ByVal numB As Integer, ByVal But1 As String, ByVal But2 As String, ByVal But3 As String, ByVal head As String)
        Dim msg As New CustomMessageBox(mes, numB, But1, But2, But3, head)
        Dim result = msg.ShowDialog()
        Dim Ans As Integer
        If result = Windows.Forms.DialogResult.Yes Then
            'user clicked "B1"
            Ans = 6
        ElseIf result = Windows.Forms.DialogResult.No Then
            'user clicked "B2"
            Ans = 7
        ElseIf result = Windows.Forms.DialogResult.Cancel Then
            'user clicked "B3"
            Ans = 8
        Else
            'user closed the window without clicking a button
            Ans = -1
            Close()
        End If
        Return Ans
    End Function 'custom MsgBox

    'WC7FULL to WC7
    Private Shared Function HexStringToByteArray(ByRef strInput As String) As Byte()
        Dim length As Integer
        Dim bOutput As Byte()
        Dim c(1) As Integer
        length = strInput.Length / 2
        ReDim bOutput(length - 1)
        For i As Integer = 0 To (length - 1)
            For j As Integer = 0 To 1
                c(j) = Asc(strInput.Chars(i * 2 + j))
                If ((c(j) >= Asc("0")) And (c(j) <= Asc("9"))) Then
                    c(j) = c(j) - Asc("0")
                ElseIf ((c(j) >= Asc("A")) And (c(j) <= Asc("F"))) Then
                    c(j) = c(j) - Asc("A") + &HA
                ElseIf ((c(j) >= Asc("a")) And (c(j) <= Asc("f"))) Then
                    c(j) = c(j) - Asc("a") + &HA
                End If
            Next j
            bOutput(i) = (c(0) * &H10 + c(1))
        Next i
        Return (bOutput)
    End Function
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        OpenFileDialog1.Filter = "wc7full files (*.wc7full)|*.wc7full|All files (*.*)|*.*"
        OpenFileDialog1.ShowDialog()
        Dim myFile As String = OpenFileDialog1.FileName
        Dim ext() As String = myFile.Split(".")

        If ext(UBound(ext)) = "wc7full" Or ext(UBound(ext)) = "WC7FULL" Then
            'Dim myFile As String = "..\..\0264 USUM - Item Roto Catch x1 (JPN).wc7full"
            Dim myBytes As Byte() = My.Computer.FileSystem.ReadAllBytes(myFile)
            Dim txtTemp As New System.Text.StringBuilder()
            For Each myByte As Byte In myBytes
                txtTemp.Append(myByte.ToString("X2"))
            Next
            RichTextBox1.Text = txtTemp.ToString()

            stat = 1
            buttons()
        Else
            MsgBox("Not a WC7FULL file", MsgBoxStyle.OkOnly)
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        SaveFileDialog1.Filter = "wc7 files (*.wc7)|*.wc7|All files (*.*)|*.*"
        SaveFileDialog1.ShowDialog()
        Dim myFile As String = SaveFileDialog1.FileName

        'Dim myFile As String = "..\..\0264 USUM - Item Roto Catch x1 (JPN) 2.wc7"
        Dim myBytes As Byte() = HexStringToByteArray(RichTextBox1.Text)
        My.Computer.FileSystem.WriteAllBytes(myFile, myBytes, False)

        stat = 0
        buttons()
    End Sub
    Private Sub cut()
        Dim SizeWC = 528
        Dim SizeFull = 1568
        Dim data As String = RichTextBox1.Text
        Debug.Print(data.Length)
        If (data.Length = SizeFull) Then
            data = data.Skip(SizeFull - SizeWC).ToArray()
        End If
        RichTextBox1.Text = data
        MsgBox("Successfully Converted", MsgBoxStyle.OkOnly)
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        cut()
        stat = 2
        buttons()
    End Sub
    Private Sub buttons()
        If stat = 0 Then
            Button1.Enabled = True
            Button2.Enabled = False
            Button3.Enabled = False
        ElseIf stat = 1 Then
            Button1.Enabled = True
            Button2.Enabled = False
            Button3.Enabled = True
        ElseIf stat = 2 Then
            Button1.Enabled = True
            Button2.Enabled = True
            Button3.Enabled = False
        End If
    End Sub

    'Script Maker
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
            MsgBox("Missing Value(s)", MsgBoxStyle.OkOnly)
        Else
            System.IO.File.WriteAllText(prog, My.Resources.PKSMScript)
            ' web.DownloadFileAsync(New Uri("https://github.com/BernardoGiordano/PKSM-Tools/raw/master/PKSMScript/PKSMScript.py"), prog)
            data(0) = TextBox1.Text
            If TextBox2.Text.Contains("0x") Then
                data(1) = TextBox2.Text
            Else
                data(1) = "0x" & TextBox2.Text
            End If
            If TextBox4.Text.Contains("0x") Then
                data(2) = TextBox4.Text
            Else
                data(2) = "0x" & TextBox4.Text
            End If
            data(3) = TextBox3.Text
            If data(3).Contains("C:\") Then
                Dim da() As String = data(3).Split("\")
                da(UBound(da)) = da(UBound(da)).Replace(" ", "_")
                If System.IO.File.Exists(pathexe & "\" & da(UBound(da))) Then
                Else
                    System.IO.File.Copy(data(3), pathexe & "\" & da(UBound(da)))
                End If
                Dim d2 As String = da(UBound(da))
                sm = "@py -3 PKSMScript.py """ & data(0) & """ -i " & data(1) & " " & data(2) & " " & """" & d2 & """" & " 1
@PUSHD %1
@MOVE *.pksm output
@del PKSMScript.py
@del " & da(UBound(da)) & "
@del output.bat"
            Else
                sm = "@py -3 PKSMScript.py """ & data(0) & """ -i " & data(1) & " " & data(2) & " " & data(3) & " 1
@PUSHD %1
@MOVE *.pksm output
@del PKSMScript.py
@del output.bat"
            End If
            System.IO.File.WriteAllText(pathexe & "\output.bat", sm)
            My.Computer.FileSystem.CreateDirectory(pathexe & "\output")
            Process.Start(pathexe & "\output.bat")
            System.Threading.Thread.Sleep(1000)
            MsgBox("Done", 0)
        End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        OpenFileDialog2.Filter = "wc7 files (*.wc7)|*.wc7|bin files (*.bin)|*.bin|txt files (*.txt)|*.txt|All files (*.*)|*.*"
        OpenFileDialog2.ShowDialog()
        TextBox3.Text = OpenFileDialog2.FileName
    End Sub

    'Info
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text = "Wonder Card Slot" Then
            ComboBox2.Enabled = True
        Else
            ComboBox2.Enabled = False
        End If
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.Text = "1" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x65D00"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x65E08"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x65F10"
        ElseIf ComboBox2.Text = "4" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66018"
        ElseIf ComboBox2.Text = "5" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66120"
        ElseIf ComboBox2.Text = "6" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66228"
        ElseIf ComboBox2.Text = "7" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66330"
        ElseIf ComboBox2.Text = "8" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66438"
        ElseIf ComboBox2.Text = "9" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66540"
        ElseIf ComboBox2.Text = "10" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66648"

        ElseIf ComboBox2.Text = "1" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66300"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66408"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66510"
        ElseIf ComboBox2.Text = "4" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66618"
        ElseIf ComboBox2.Text = "5" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66720"
        ElseIf ComboBox2.Text = "6" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66828"
        ElseIf ComboBox2.Text = "7" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66930"
        ElseIf ComboBox2.Text = "8" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66A38"
        ElseIf ComboBox2.Text = "9" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66B40"
        ElseIf ComboBox2.Text = "10" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66C48"
        End If

    End Sub
    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        ComboBox1.Enabled = True
        ComboBox2_SelectedIndexChanged(sender, e)
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        TabControl1.TabIndex -= 2
        TextBox2.Text = Label5.Text
        If ComboBox1.Text = "Wonder Card Slot" Then
            TextBox4.Text = "0x108"
        End If
        TabControl1.TabIndex += 2
    End Sub
    Private Sub Label5_TextChanged(sender As Object, e As EventArgs) Handles Label5.TextChanged
        Button6.Enabled = True
    End Sub
End Class