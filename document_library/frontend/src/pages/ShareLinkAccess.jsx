import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

const BACKEND_URL = "http://localhost:5113"; 

function ShareLinkAccess() {
  const { token } = useParams();
  const [status, setStatus] = useState('Validating...');
  const [documentId, setDocumentId] = useState(null);

  useEffect(() => {
    const validateLink = async () => {
      try {
        setStatus('Validating...');
        const res = await fetch(`${BACKEND_URL}/api/share/${token}/validate`, {
          cache: "no-store"
        });
        if (!res.ok) {
          const text = await res.text();
          throw new Error(text);
        }
        const data = await res.json();
        setDocumentId(data.documentId);
        setStatus('✅ Link is valid!');
      } catch (err) {
        console.error(err);
        setStatus('❌ Link is invalid or expired.');
      }
    };

    validateLink();
  }, [token]);

  return (
    <div style={{ padding: '2rem', fontFamily: 'Arial, sans-serif' }}>
      <h2>🔗 Access Shared Document</h2>
      <p>{status}</p>
      {documentId && (
        <a
          href={`${BACKEND_URL}/api/share/${token}`}
          download
          style={{
            display: 'inline-block',
            marginTop: '1rem',
            padding: '0.5rem 1rem',
            backgroundColor: '#007bff',
            color: 'white',
            textDecoration: 'none'
          }}
        >
          📥 Download Document
        </a>
      )}
    </div>
  );
}

export default ShareLinkAccess;
