Public Class options
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text = "ON" Then
            My.Settings.AutoUpdating = True
        ElseIf ComboBox1.Text = "OFF" Then
            My.Settings.AutoUpdating = False
        End If
    End Sub
    Private Sub options_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ComboBox1.Items.Clear()
        ComboBox1.Items.Add("ON")
        ComboBox1.Items.Add("OFF")
        If My.Settings.AutoUpdating = True Then
            ComboBox1.Text = "ON"
        ElseIf My.Settings.AutoUpdating = False Then
            ComboBox1.Text = "OFF"
        End If
        ComboBox1_SelectedIndexChanged(sender, e)
        Me.Refresh()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class