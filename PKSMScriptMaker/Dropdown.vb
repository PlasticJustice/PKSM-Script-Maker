Public Class LangMessageBox
    Dim sel As String
    Public Sub New(ByVal button1Name As String, ByVal button2name As String, ByVal labeltext As String, ByVal dropdowntext As String, ByVal options As String(), ByVal head As String)
        InitializeComponent()
        Me.Text = head
        Button1.Text = button1Name
        Button2.Text = button2name
        ComboBox1.Text = "----" & dropdowntext & "----"
        Label1.Text = labeltext
        ComboBox1.Items.Clear()
        For i = 1 To UBound(options) Step 1
            ComboBox1.Items.Add(options(i))
        Next i
        If labeltext = "" Then
            ComboBox1.Location = New Point(80, 23)
            Label1.Hide()
        ElseIf labeltext <> "" Then
            ComboBox1.Location = New Point(80, 43)
        End If
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        My.Settings.ddres = sel
        DialogResult = Windows.Forms.DialogResult.Yes
        Close()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        DialogResult = Windows.Forms.DialogResult.No
        Close()
        Form1.Close()
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Items.Contains(ComboBox1.Text) = True Then
            sel = ComboBox1.Text
            Button1.Enabled = True
        Else
            sel = ComboBox1.Text
            Button1.Enabled = False
        End If
    End Sub
End Class