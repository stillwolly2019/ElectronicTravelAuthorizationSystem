function UploadComplete(sender, args) {
    if (parseInt(args.get_length()) < 4194304) {
        var tokens = args.get_fileName().match(/(.*?)\.(doc|docx|pdf|xls|xlsx)$/);

        if (tokens != null) {
            var fileName = $get("ContentPlaceHolder1_hfFileLoad").value;
            $get("ContentPlaceHolder1_hfFileLoad").value = fileName;

            var FileExtension = $get("ContentPlaceHolder1_hdnlblFileExt").value;
            $get("ContentPlaceHolder1_hdnlblFileExt").value = FileExtension;

            var FileNameOnly = $get("ContentPlaceHolder1_hdnNameOnly").value;
            $get("ContentPlaceHolder1_hdnNameOnly").value = FileNameOnly;


            $get("ContentPlaceHolder1_lblUploadMsg").innerHTML = "<span style='color:Green;'>File was uploaded successfully</span>";
            $get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").value = "";
            $get("ContentPlaceHolder1_pnlFile").style.display = "";
            $get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.display = "none";
        }
        else {
            $get("ContentPlaceHolder1_lblUploadMsg").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
            $get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.backgroundColor = "Red";
        }

    }
    else {
        $get("ContentPlaceHolder1_hfFileLoad").innerHTML = "<span style='color:Red;'>File Size Should be less than 4MB</span>";
        $get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.backgroundColor = "Red";
        return;
    }
}

function UploadError(sender, args) {
    $get("ContentPlaceHolder1_lblUploadMsg").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
    $get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.backgroundColor = "Red";
}

function StartUpload(sender, args) {
    $get("ContentPlaceHolder1_lblUploadMsg").innerText = "";
    //$get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.display = "none";
    $get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.backgroundColor = "White";
}



//region Upload Medical

function UploadCompleteMedical(sender, args) {
    if (parseInt(args.get_length()) < 4194304) {
        var tokens = args.get_fileName().match(/(.*?)\.(doc|docx|pdf|xls|xlsx)$/);
        if (tokens != null) {
            var fileName = $get("hfFileUploadMedical").value;
            $get("hfFileUploadMedical").value = args.get_fileName();

            var FileExtension = $get("hdnlblFileExtMedical").value;
            $get("hdnlblFileExtMedical").value = args.FileExtension;

            var FileNameOnly = $get("hdnNameOnlyMedical").value;
            $get("hdnNameOnlyMedical").value = args.FileNameOnly;
            

            $get("lblUploadMessageMedical").innerHTML = "<span style='color:Green;'>File was uploaded successfully</span>";
            $get("fuMedicalAttachement_ctl02").value = "";
            $get("pnlMedicalFiles").style.display = "";
            $get("fuMedicalAttachement_ctl02").style.display = "none";
        }
        else
        {
            $get("lblUploadMessageMedical").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
            $get("fuMedicalAttachement_ctl02").style.backgroundColor = "Red";
        }

    }
    else {
        $get("hfFileUploadMedical").innerHTML = "<span style='color:Red;'>File Size Should be less than 4MB</span>";
        $get("fuMedicalAttachement_ctl02").style.backgroundColor = "Red";
        return;
    }
}

function UploadErrorMedical(sender, args) {
    $get("lblUploadMessageMedical").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
    $get("fuMedicalAttachement_ctl02").style.backgroundColor = "Red";
}

function StartUploadMedical(sender, args) {
    $get("lblUploadMessageMedical").innerText = "";
    //$get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.display = "none";
    $get("fuMedicalAttachement_ctl02").style.backgroundColor = "White";
}


//End region Upload Medical



//region Upload TEC

function UploadCompleteTEC(sender, args) {
    if (parseInt(args.get_length()) < 4194304) {
        var tokens = args.get_fileName().match(/(.*?)\.(doc|docx|pdf|xls|xlsx)$/);
        if (tokens != null) {
            var fileName = $get("ContentPlaceHolder1_hfFileLoadTEC").value;
            $get("ContentPlaceHolder1_hfFileLoadTEC").value = fileName;

            var FileExtension = $get("ContentPlaceHolder1_hdnlblFileExtTEC").value;
            $get("ContentPlaceHolder1_hdnlblFileExtTEC").value = FileExtension;

            var FileNameOnly = $get("ContentPlaceHolder1_hdnNameOnlyTEC").value;
            $get("ContentPlaceHolder1_hdnNameOnlyTEC").value = FileNameOnly;


            $get("ContentPlaceHolder1_lblUploadMessageTEC").innerHTML = "<span style='color:Green;'>File was uploaded successfully</span>";
            $get("ctl00_ContentPlaceHolder1_fuAttachmentTEC_ctl02").value = "";
            $get("ContentPlaceHolder1_pnlFileTEC").style.display = "";
            $get("ctl00_ContentPlaceHolder1_fuAttachmentTEC_ctl02").style.display = "none";
        }
        else {
            $get("ContentPlaceHolder1_lblUploadMessageTEC").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
            $get("ctl00_ContentPlaceHolder1_fuAttachmentTEC_ctl02").style.backgroundColor = "Red";
        }
    }
    else {
        $get("ContentPlaceHolder1_hfFileLoadTEC").innerHTML = "<span style='color:Red;'>File Size Should be less than 4MB</span>";
        $get("ctl00_ContentPlaceHolder1_fuAttachmentTEC_ctl02").style.backgroundColor = "Red";
        return;
    }
}

function UploadErrorTEC(sender, args) {
    $get("ContentPlaceHolder1_lblUploadMessageTEC").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
    $get("ctl00_ContentPlaceHolder1_fuAttachmentTEC_ctl02").style.backgroundColor = "Red";
}

function StartUploadTEC(sender, args) {
    $get("ContentPlaceHolder1_lblUploadMessageTEC").innerText = "";
    //$get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.display = "none";
    $get("ctl00_ContentPlaceHolder1_fuAttachmentTEC_ctl02").style.backgroundColor = "White";
}

//End region Upload TEC