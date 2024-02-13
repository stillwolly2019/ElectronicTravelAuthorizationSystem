
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
            //$get("lblUploadMsg").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
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
    $get("lblUploadMsg").innerHTML = "<span style='color:Red;'>The only file types allowed is (pdf)</span>";
    $get("fuAttachment_ctl02").style.backgroundColor = "Red";
    $get("LoadingImg").style.display = "none";
}

function StartUpload(sender, args) {
    $get("LoadingImg").style.display = "";
    $get("lblUploadMsg").innerText = "";
    //$get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.display = "none";
    $get("fuAttachment_ctl02").style.backgroundColor = "White";
}



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

            $get("lblUploadMessageTEC").innerHTML = "<span style='color:Green;'>File was uploaded successfully</span>";
            $get("fuAttachmentTEC_ctl02").value = "";
            $get("pnlFileTEC").style.display = "";
            $get("fuAttachmentTEC_ctl02").style.display = "none";
            $get("LoadingImg").style.display = "none";
        }
        else {
            $get("lblUploadMessageTEC").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
            $get("fuAttachmentTEC_ctl02").style.backgroundColor = "Red";
            $get("LoadingImg").style.display = "none";
        }
    }
    else {
        $get("hfFileLoadTEC").innerHTML = "<span style='color:Red;'>File Size Should be less than 4MB</span>";
        $get("fuAttachmentTEC_ctl02").style.backgroundColor = "Red";
        $get("LoadingImg").style.display = "none";
        return;
    }
}
function UploadErrorTEC(sender, args) {
    $get("lblUploadMessageTEC").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
    $get("fuAttachmentTEC_ctl02").style.backgroundColor = "Red";
    $get("LoadingImg").style.display = "none";
}
function StartUploadTEC(sender, args) {
    $get("LoadingImg").style.display = "";
    $get("lblUploadMessageTEC").innerText = "";
    $get("fuAttachmentTEC_ctl02").style.backgroundColor = "White";
}
//End region Upload TEC

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
    $get("lblUploadMessageOtherDocuments").innerHTML = "<span style='color:Red;'>The only file types allowed are (pdf)</span>";
    $get("fuOtherDocumentsAttachement_ctl02").style.backgroundColor = "Red";
    $get("LoadingImgOtherDocuments").style.display = "none";
}
function StartUploadOtherDocuments(sender, args) {
    $get("LoadingImgOtherDocuments").style.display = "";
    $get("lblUploadMessageOtherDocuments").innerText = "";
    $get("fuOtherDocumentsAttachement_ctl02").style.backgroundColor = "White";
}

