import { useEffect, useState } from 'react';
import './App.css';
import { List, ListItem, ListItemText, Typography } from '@mui/material';

function App() {
  const [meetings, setMeetings] = useState<Meeting[]>([]);

  useEffect(() => {
    fetch('https://localhost:5001/api/meetings')
      .then((response) => response.json())
      .then((data) => setMeetings(data));

    return () => {};
  }, []);

  return (
    <>
      <Typography variant='h3'>Meetings</Typography>
      <List>
        {meetings.map((meeting) => (
          <ListItem key={meeting.id}>
            <ListItemText>{meeting.title}</ListItemText>
          </ListItem>
        ))}
      </List>
    </>
  );
}

export default App;
