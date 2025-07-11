import React, { useState } from 'react';
import { createShareLink } from '../api/api';

function ShareLinkGenerator({ documentId }) {
  const [hours, setHours] = useState(1);
  const [generatedLink, setGeneratedLink] = useState('');
  const [status, setStatus] = useState('');

  const handleGenerate = async () => {
    if (!hours || hours <= 0) {
      setStatus('Please enter a valid number of hours.');
      return;
    }

    try {
      setStatus('Generating...');
      const result = await createShareLink(documentId, hours);
      const shareToken = result.token;
      const shareUrl = `http://localhost:5173/share/${shareToken}`;
      setGeneratedLink(shareUrl);
      setStatus('✅ Link generated!');
    } catch (err) {
      setStatus('❌ Error generating share link.');
    }
  };

  return (
    <div style={{ marginTop: '0.5rem', padding: '0.5rem', border: '1px dashed #ccc' }}>
      <label>
        Expires in (hours):
        <input
          type="number"
          value={hours}
          onChange={(e) => setHours(parseInt(e.target.value))}
          min="1"
          style={{ width: '60px', marginLeft: '0.5rem' }}
        />
      </label>
      <button onClick={handleGenerate} style={{ marginLeft: '1rem' }}>
        Generate Share Link
      </button>
      {status && <p>{status}</p>}
      {generatedLink && (
        <div>
          <p>Share this link:</p>
          <code>{generatedLink}</code>
        </div>
      )}
    </div>
  );
}

export default ShareLinkGenerator;