import { SearchOff } from '@mui/icons-material';
import { Button, Paper, Typography } from '@mui/material';
import { Link } from 'react-router';

export default function NotFound() {
  return (
    <Paper
      sx={{
        height: 400,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        p: 6,
      }}
    >
      <SearchOff color='primary' sx={{ fontSize: 100 }} />
      <Typography variant='h3' color='primary' gutterBottom>
        Oops - we've looked everywhere but we couldn't find what you were
        looking for.
      </Typography>
      <Typography variant='h5' color='secondary'>
        (404) Not Found
      </Typography>

      <Button to={'/activities'} fullWidth component={Link}>
        Return to the activities page
      </Button>
    </Paper>
  );
}
