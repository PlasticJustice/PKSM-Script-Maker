Public Class about
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim webAddress As String = "https://github.com/PlasticJustice/PKSM-Script-Maker"
        Process.Start(webAddress)
    End Sub
    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Dim webAddress As String = "https://github.com/BernardoGiordano/PKSM-Tools/tree/master/PKSMScript"
        Process.Start(webAddress)
    End Sub
    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Dim webAddress As String = "https://github.com/BernardoGiordano/PKSM"
        Process.Start(webAddress)
    End Sub
    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        Dim webAddress As String = "https://www.python.org/downloads/"
        Process.Start(webAddress)
    End Sub
End Class