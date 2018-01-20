Public Class options
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text = "ON" Then
            My.Settings.AutoUpdating = True
        ElseIf ComboBox1.Text = "OFF" Then
            My.Settings.AutoUpdating = False
        End If
    End Sub
End Class