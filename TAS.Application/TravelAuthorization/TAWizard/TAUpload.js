
//region Upload Security
function UploadComplete(sender, args) {
    if (parseInt(args.get_length()) < 4194304) {
        var tokens = args.get_fileName().match(/(.*?)\.(pdf)$/);
        //var tokens = args.get_fileName().match(/(.*?)\.(doc|docx|pdf|xls|xlsx)$/);

        if (tokens != null) {
            $get("lblUploadMsg").innerHTML = "<span style='color:Green;'>File was uploaded successfully</span>";
            $get("fuAttachment_ctl02").value = "";
            $get("pnlFile").style.display = "";
            $get("fuAttachment_ctl02").style.display = "none";
            $get("LoadingImg").style.display = "none";
        }
        else {
            $get("lblUploadMsg").innerHTML = "<span style='color:Red;'>The only file type allowed is (pdf)</span>";
            $get("fuAttachment_ctl02").style.backgroundColor = "Red";
            $get("LoadingImg").style.display = "none";
        }

    }
    else {
        $get("lblUploadMsg").innerHTML = "<span style='color:Red;'>File Size Should be less than 4MB</span>";
        $get("fuAttachment_ctl02").style.backgroundColor = "Red";
        $get("LoadingImg").style.display = "none";
        return;
    }
}
function UploadError(sender, args) {
    $get("lblUploadMsg").innerHTML = "<span style='color:Red;'>The only file type allowed is (pdf)</span>";
    $get("fuAttachment_ctl02").style.backgroundColor = "Red";
    $get("LoadingImg").style.display = "none";
}
function StartUpload(sender, args) {
    $get("LoadingImg").style.display = "";
    $get("lblUploadMsg").innerText = "";
    $get("fuAttachment_ctl02").style.backgroundColor = "White";
}
//End region Upload Security

//region Upload Medical
function UploadCompleteMedical(sender, args) {
    if (parseInt(args.get_length()) < 4194304) {
        var tokens = args.get_fileName().match(/(.*?)\.(doc|docx|pdf|xls|xlsx)$/);
        if (tokens != null) {

            $get("lblUploadMessageMedical").innerHTML = "<span style='color:Green;'>File was uploaded successfully</span>";
            $get("fuMedicalAttachement_ctl02").value = "";
            $get("pnlMedicalFiles").style.display = "";
            $get("fuMedicalAttachement_ctl02").style.display = "none";
            $get("LoadingImgMed").style.display = "none";
        }
        else {
            $get("lblUploadMessageMedical").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
            $get("fuMedicalAttachement_ctl02").style.backgroundColor = "Red";
            $get("LoadingImgMed").style.display = "none";
        }

    }
    else {
        $get("hfFileUploadMedical").innerHTML = "<span style='color:Red;'>File Size Should be less than 4MB</span>";
        $get("fuMedicalAttachement_ctl02").style.backgroundColor = "Red";
        $get("LoadingImgMed").style.display = "none";
        return;
    }
}
function UploadErrorMedical(sender, args) {
    $get("lblUploadMessageMedical").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
    $get("fuMedicalAttachement_ctl02").style.backgroundColor = "Red";
    $get("LoadingImgMed").style.display = "none";
}
function StartUploadMedical(sender, args) {
    $get("LoadingImgMed").style.display = "";
    $get("lblUploadMessageMedical").innerText = "";
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

//region Upload Training
function UploadCompleteTraining(sender, args) {
    if (parseInt(args.get_length()) < 4194304) {
        var tokens = args.get_fileName().match(/(.*?)\.(doc|docx|pdf|xls|xlsx)$/);
        if (tokens != null) {

            $get("lblUploadMessageTraining").innerHTML = "<span style='color:Green;'>File was uploaded successfully</span>";
            $get("fuTrainingAttachement_ctl02").value = "";
            $get("pnlTrainingFiles").style.display = "";
            $get("fuTrainingAttachement_ctl02").style.display = "none";
            $get("LoadingImgTraining").style.display = "none";
        }
        else {
            $get("lblUploadMessageTraining").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
            $get("fuTrainingAttachement_ctl02").style.backgroundColor = "Red";
            $get("LoadingImgTraining").style.display = "none";
        }

    }
    else {
        $get("hfFileUploadTraining").innerHTML = "<span style='color:Red;'>File Size Should be less than 4MB</span>";
        $get("fuTrainingAttachement_ctl02").style.backgroundColor = "Red";
        $get("LoadingImgTraining").style.display = "none";
        return;
    }
}
function UploadErrorTraining(sender, args) {
    $get("lblUploadMessageTraining").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
    $get("fuTrainingAttachement_ctl02").style.backgroundColor = "Red";
    $get("LoadingImgTraining").style.display = "none";
}
function StartUploadTraining(sender, args) {
    $get("LoadingImgTraining").style.display = "";
    $get("lblUploadMessageTraining").innerText = "";
    $get("fuTrainingAttachement_ctl02").style.backgroundColor = "White";
}
//End region Upload Training

//region Upload OtherDocuments
function UploadCompleteOtherDocuments(sender, args) {
    if (parseInt(args.get_length()) < 4194304) {
        var tokens = args.get_fileName().match(/(.*?)\.(doc|docx|pdf|xls|xlsx)$/);
        if (tokens != null) {

            $get("lblUploadMessageOtherDocuments").innerHTML = "<span style='color:Green;'>File was uploaded successfully</span>";
            $get("fuOtherDocumentsAttachement_ctl02").value = "";
            $get("pnlOtherDocumentsFiles").style.display = "";
            $get("fuOtherDocumentsAttachement_ctl02").style.display = "none";
            $get("LoadingImgOtherDocuments").style.display = "none";
        }
        else {
            $get("lblUploadMessageOtherDocuments").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
            $get("fuOtherDocumentsAttachement_ctl02").style.backgroundColor = "Red";
            $get("LoadingImgOtherDocuments").style.display = "none";
        }

    }
    else {
        $get("lblUploadMessageOtherDocuments").innerHTML = "<span style='color:Red;'>File Size Should be less than 4MB</span>";
        $get("fuOtherDocumentsAttachement_ctl02").style.backgroundColor = "Red";
        $get("LoadingImgOtherDocuments").style.display = "none";
        return;
    }
}
function UploadErrorOtherDocuments(sender, args) {
    $get("lblUploadMessageOtherDocuments").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
    $get("fuOtherDocumentsAttachement_ctl02").style.backgroundColor = "Red";
    $get("LoadingImgOtherDocuments").style.display = "none";
}
function StartUploadOtherDocuments(sender, args) {
    $get("LoadingImgOtherDocuments").style.display = "";
    $get("lblUploadMessageOtherDocuments").innerText = "";
    $get("fuOtherDocumentsAttachement_ctl02").style.backgroundColor = "White";
}
//region Upload OtherDocuments














