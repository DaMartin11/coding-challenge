import React, { useState } from "react";
import { uploadDocument, uploadMultipleDocuments } from "../api/api";

function DocumentUpload({ onUploadComplete }) {
  const [selectedFiles, setSelectedFiles] = useState([]);
  const [status, setStatus] = useState("");

  const handleFileChange = (e) => {
    setSelectedFiles(Array.from(e.target.files));
    setStatus("");
  };

  const handleUploadSingle = async () => {
    if (selectedFiles.length === 0) {
      setStatus("Please select a file.");
      return;
    }
    try {
      await uploadDocument(selectedFiles[0]);
      setStatus("✅ Single document uploaded successfully!");
      onUploadComplete && onUploadComplete();
    } catch (error) {
      setStatus("❌ Error uploading single document.");
    }
  };

  const handleUploadMultiple = async () => {
    if (selectedFiles.length === 0) {
      setStatus("Please select files.");
      return;
    }
    try {
      await uploadMultipleDocuments(selectedFiles);
      setStatus("✅ Multiple documents uploaded successfully!");
      onUploadComplete && onUploadComplete();
    } catch (error) {
      console.log(error);
      setStatus("❌ Error uploading multiple documents.");
    }
  };

  return (
    <div
      style={{ padding: "1rem", border: "1px solid #ccc", marginTop: "1rem" }}
    >
      <h2>📂 Upload Documents</h2>
      <input type="file" multiple onChange={handleFileChange} />
      <div style={{ marginTop: "1rem" }}>
        <button onClick={handleUploadSingle} style={{ marginRight: "1rem" }}>
          Upload Single
        </button>
        <button onClick={handleUploadMultiple}>Upload Multiple</button>
      </div>
      {status && <p>{status}</p>}
    </div>
  );
}

export default DocumentUpload;
