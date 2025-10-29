import React from 'react';
import Box from '@mui/material/Box/Box';
import { Button, Toolbar, Typography, useTheme } from '@mui/material';

import { SubscriptionState } from '../service/SubscriptionState';
import { SessionState } from '../service/SessionState';
import { SessionContext } from '../SessionContext';
import { SubscriptionContext } from '../SubscriptionContext';

export const SessionStatusBar = () => {
   const theme = useTheme();

   const {
      sessionState,
      setIsEnabled,
      setIsSessionEnabled
   } = React.useContext(SessionContext);

   const {
      setIsSubscriptionEnabled,
      subscriptionState,
      lastSequenceNumber,
   } = React.useContext(SubscriptionContext);

    const handleConnect = React.useCallback((state: SessionState) => {
        if (state === SessionState.Disconnected || state === SessionState.NoSession) {
            setIsSubscriptionEnabled(true);
            setIsSessionEnabled(true);
            setIsEnabled(true);
        }
        else if (state === SessionState.SessionActive || state == SessionState.Error) {
            setIsSubscriptionEnabled(false);
            setIsSessionEnabled(false);
            setIsEnabled(false);
        }
    }, [setIsEnabled, setIsSessionEnabled, setIsSubscriptionEnabled]);

   /***
    * Handle subscription state changes
    * @param subscriptionState
    * 
    * This function is called when the subscription state changes.
    */
    const handleSubscription = React.useCallback((subscriptionState: SubscriptionState) => {
        if (subscriptionState === SubscriptionState.Closed) {
            setIsSubscriptionEnabled(true);
        }
        else {
            setIsSubscriptionEnabled(false);
        }
    }, [setIsSubscriptionEnabled]);

   return (
      <Toolbar variant='dense' disableGutters sx={{ py: 0, minHeight: '36px', justifyContent: 'space-between' }}>
         <Box ml={6} sx={{ flexGrow: 0, display: { xs: 'none', color: theme.palette.text.primary, md: 'flex' } }}>
               {/* <CustomButton sx={{ mr: 2 }} onClick={() => handleConnect(sessionState)}>
                    {(sessionState === SessionState.Disconnected) ? <PlayArrowIcon /> : <StopIcon />}
                </CustomButton> */}
            <Button sx={{ mr: 2 }} onClick={() => handleConnect(sessionState)} >
               <Typography variant='body2' sx={{ pr: 4 }}>Websocket:</Typography>
               <Typography variant='body2' fontWeight={'bolder'}>{SessionState[sessionState]}</Typography>
            </Button>
            <Button sx={{ my: 2 }} onClick={() => handleSubscription(subscriptionState)}>
               <Typography variant='body2' sx={{ pr: 4 }}>Subscription:</Typography>
               <Typography variant='body2' fontWeight={'bolder'}>{SubscriptionState[subscriptionState]}</Typography>
            </Button>
            <Button sx={{ my: 2 }}>
               <Typography variant='body2' sx={{ pr: 4 }}>Last Publish:</Typography>
               <Typography variant='body2' fontWeight={'bolder'}>{lastSequenceNumber ?? '---'}</Typography>
            </Button>
         </Box>
      </Toolbar>
   );
}

export default SessionStatusBar;