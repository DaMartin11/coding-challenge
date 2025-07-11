import React, { useEffect, useState } from "react";
import { getDocuments, getDownloadUrl } from "../api/api";
import ShareLinkGenerator from "./ShareLinkGenerator";

function DocumentList({ refreshKey, onRequestRefresh }) {
  const [documents, setDocuments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    fetchDocuments();
  }, [refreshKey]);

  const fetchDocuments = async () => {
    try {
      setLoading(true);
      const docs = await getDocuments();
      setDocuments(docs);
    } catch (err) {
      setError("Failed to load documents.");
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <p>Loading documents...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;

  return (
    <div style={{ padding: "1rem", marginTop: "2rem" }}>
      <h2>📃 Document List</h2>
      {documents.length === 0 && <p>No documents uploaded yet.</p>}
      <ul style={{ listStyle: "none", padding: 0 }}>
        {documents.map(doc => (
  <li key={doc.id} style={{ marginBottom: '1rem', border: '1px solid #ddd', padding: '1rem' }}>
    <h4>{doc.name}</h4>

    {doc.previewUrl || doc.iconUrl ? (
      <img
        src={doc.previewUrl || doc.iconUrl}
        alt="Preview"
        style={{ width: '150px', height: 'auto', marginBottom: '0.5rem' }}
      />
    ) : null}

    <p>Type: {doc.type}</p>
    <p>Upload Date: {new Date(doc.uploadDate).toLocaleString()}</p>
    <p>Downloads: {doc.downloadCount}</p>

  <button
  onClick={() => {
    window.open(getDownloadUrl(doc.id), '_blank');
    onRequestRefresh && onRequestRefresh();
  }}
>
  Download
</button>
    <ShareLinkGenerator documentId={doc.id} />
  </li>
))}

      </ul>
    </div>
  );
}

export default DocumentList;
