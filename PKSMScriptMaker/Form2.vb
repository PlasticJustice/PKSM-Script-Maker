Public Class Form2
    Dim ep As Integer
    Private Sub RichTextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles RichTextBox1.KeyPress

        '97 - 122 = Ascii codes for simple letters
        '65 - 90  = Ascii codes for capital letters
        '48 - 57  = Ascii codes for numbers

        If Asc(e.KeyChar) = 10 Then
            If ep <= 3 Then
                ep = ep + 1
            Else
                e.Handled = True
            End If
        End If
        If Asc(e.KeyChar) = 8 Then
            ep = 0
            Dim texto As String = RichTextBox1.Text
            Dim chr() As Char = texto.ToCharArray
            For i = 0 To UBound(chr) Step 1
                Dim int As Integer = Asc(chr(i))
                If int = 10 Then
                    ep = ep + 1
                End If
            Next i
        End If
        If Asc(e.KeyChar) = 60 Or Asc(e.KeyChar) = 62 Or Asc(e.KeyChar) = 91 Or Asc(e.KeyChar) = 92 Or Asc(e.KeyChar) = 93 Or Asc(e.KeyChar) = 94 Or Asc(e.KeyChar) = 96 Or Asc(e.KeyChar) = 123 Or Asc(e.KeyChar) = 124 Or Asc(e.KeyChar) = 125 Then
            e.Handled = True
        End If
        'SendKeys.Send("_")
        'RichTextBox1.Refresh()
        'hx(False)
    End Sub
    'Private Sub RichTextBox1_TC(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles RichTextBox1.TextChanged
    '    System.Threading.Thread.Sleep(750)
    '    RichTextBox1.Refresh()
    '    hx(False)
    'End Sub
    Public Function hx(ByVal title As Boolean)
        Dim texto As String = Nothing
        Dim n As Integer
        'Dim h() As String
        If title = True Then
            'Dim h(36) As String
            n = 36
        ElseIf title = False Then
            RichTextBox2.Text = Nothing
            texto = RichTextBox1.Text
            n = 194
        End If
        Dim h(n) As String
        Dim chr() As Char = texto.ToCharArray
        For i = 0 To UBound(chr) Step 1
            Dim int As Integer = Asc(chr(i))
            '97 - 122 = Ascii codes for simple letters
            '65 - 90  = Ascii codes for capital letters
            '48 - 57  = Ascii codes for numbers
            If int >= 65 And int <= 90 Then
                int = int - 22
                h(i) = Hex(int) & " 01 "
            ElseIf int >= 97 And int <= 122 Then
                int = int - 28
                h(i) = Hex(int) & " 01 "
            ElseIf int >= 48 And int <= 57 Then
                int = int - 15
                h(i) = Hex(int) & " 01 "
            Else
                Select Case int
                    Case 10
                        h(i) = "00 E0 "
                    Case 32
                        h(i) = "DE" & " 01 "
                    Case 33
                        h(i) = "AB" & " 01 "
                    Case 34
                        h(i) = "B5" & " 01 "
                    Case 35
                        h(i) = "C0" & " 01 "
                    Case 36
                        h(i) = "A8" & " 01 "
                    Case 37
                        h(i) = "D2" & " 01 "
                    Case 38
                        h(i) = "C2" & " 01 "
                    Case 39
                        h(i) = "B3" & " 01 "
                    Case 40
                        h(i) = "B9" & " 01 "
                    Case 41
                        h(i) = "BA" & " 01 "
                    Case 42
                        h(i) = "BF" & " 01 "
                    Case 43
                        h(i) = "BD" & " 01 "
                    Case 44
                        h(i) = "AD" & " 01 "
                    Case 45
                        h(i) = "BE" & " 01 "
                    Case 46
                        h(i) = "AE" & " 01 "
                    Case 47
                        h(i) = "B1" & " 01 "
                    Case 58
                        h(i) = "C4" & " 01 "
                    Case 59
                        h(i) = "C5" & " 01 "
                    Case 61
                        h(i) = "C1" & " 01 "
                    Case 63
                        h(i) = "AC" & " 01 "
                    Case 64
                        h(i) = "D0" & " 01 "
                    Case 95
                        h(i) = "E9" & " 01 "
                    Case 126
                        h(i) = "C3" & " 01 "

                    Case 199
                        h(i) = "66" & " 01 "
                    Case 252
                        h(i) = "9B" & " 01 "
                    Case 233
                        h(i) = "88" & " 01 "
                    Case 226
                        h(i) = "81" & " 01 "
                    Case 228
                        h(i) = "83" & " 01 "
                    Case 224
                        h(i) = "7F" & " 01 "
                    'Case 134
                    '    h(i) = "84" & " 01 "
                    Case 231
                        h(i) = "86" & " 01 "
                    Case 234
                        h(i) = "89" & " 01 "
                    Case 235
                        h(i) = "8A" & " 01 "
                    Case 232
                        h(i) = "87" & " 01 "
                    Case 239
                        h(i) = "8E" & " 01 "
                    Case 238
                        h(i) = "8D" & " 01 "
                    Case 236
                        h(i) = "8B" & " 01 "
                    Case 196
                        h(i) = "63" & " 01 "
                    'Case 143
                    '    h(i) = "64" & " 01 "
                    Case 201
                        h(i) = "68" & " 01 "
                    'Case 145
                    '    h(i) = "85" & " 01 "
                    'Case 146
                    '    h(i) = "65" & " 01 "
                    Case 244
                        h(i) = "93" & " 01 "
                    Case 246
                        h(i) = "95" & " 01 "
                    Case 242
                        h(i) = "91" & " 01 "
                    Case 251
                        h(i) = "9A" & " 01 "
                    Case 249
                        h(i) = "98" & " 01 "
                    Case 255
                        h(i) = "9E" & " 01 "
                    Case 214
                        h(i) = "75" & " 01 "
                    Case 220
                        h(i) = "7B" & " 01 "

                End Select
            End If
        Next i

        If title = True Then

        ElseIf title = False Then
            For i = 0 To UBound(h) Step 1
                RichTextBox2.Text = RichTextBox2.Text & h(i)
            Next i
        End If
        Return 0
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        hx(False)
        Label1.ForeColor = Color.FromArgb(88, 88, 80) 'Color.RGB("58", "58", "50")
        Label1.Text = RichTextBox1.Text
    End Sub
End Class