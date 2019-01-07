Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form1



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GetIP("NASKLUMP")


        Dim test As String = GetMBServerMACAddress.GetMACAddress()


        Dim ServerUp As New WakeUpServer
        ServerUp.macAddress = "00-30-1B-BD-9B-F2"
        ServerUp.wakeIt()

    End Sub
    Private Function GetIP(ByVal DNSName As String) As String

        Try

            Return Net.Dns.GetHostEntry(DNSName).AddressList.GetLowerBound(0).ToString

        Catch ex As Exception

            Return String.Empty

        End Try

    End Function



    Private Sub SendMagicPacket()

        'SET THESE VARIABLES TO REAL VALUES

        Dim MacAddress As String = String.Empty
        Dim WANIPAddr As String = String.Empty
        Dim LanSubnet As String = String.Empty



        Dim Port As Integer = 9
        Dim udpClient As New System.Net.Sockets.UdpClient
        Dim buf(101) As Char
        Dim sendBytes As [Byte]() = System.Text.Encoding.ASCII.GetBytes(buf)

        For x As Integer = 0 To 5
            sendBytes(x) = CByte("&HFF")
        Next

        MacAddress = MacAddress.Replace("-", "").Replace(":", "")

        Dim i As Integer = 6

        For x As Integer = 1 To 16
            sendBytes(i) = CByte("&H" + MacAddress.Substring(0, 2))
            sendBytes(i + 1) = CByte("&H" + MacAddress.Substring(2, 2))
            sendBytes(i + 2) = CByte("&H" + MacAddress.Substring(4, 2))
            sendBytes(i + 3) = CByte("&H" + MacAddress.Substring(6, 2))
            sendBytes(i + 4) = CByte("&H" + MacAddress.Substring(8, 2))
            sendBytes(i + 5) = CByte("&H" + MacAddress.Substring(10, 2))

            i += 6
        Next

        Dim myAddress As String

        Try
            myAddress = Net.IPAddress.Parse(WANIPAddr).ToString
        Catch ex As Exception
            myAddress = GetIP(WANIPAddr)
        End Try


        If myAddress = String.Empty Then
            MessageBox.Show("Invalid IP address/Host Name given")
            Return
        End If

        Dim mySubnetArray() As String
        Dim sm1, sm2, sm3, sm4 As Int64

        mySubnetArray = LanSubnet.Split("."c)

        For i = 0 To mySubnetArray.GetUpperBound(0)
            Select Case i
                Case Is = 0
                    sm1 = Convert.ToInt64(mySubnetArray(i))
                Case Is = 1
                    sm2 = Convert.ToInt64(mySubnetArray(i))
                Case Is = 2
                    sm3 = Convert.ToInt64(mySubnetArray(i))
                Case Is = 3
                    sm4 = Convert.ToInt64(mySubnetArray(i))
            End Select

        Next

        myAddress = WANIPAddr
        udpClient.Send(sendBytes, sendBytes.Length, myAddress, Port)

    End Sub

   

End Class
