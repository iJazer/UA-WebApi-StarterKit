import React from 'react';
import Box from '@mui/material/Box/Box';
import PlayArrowIcon from '@mui/icons-material/PlayArrow';
import StopIcon from '@mui/icons-material/Stop';
import { Button, Toolbar, Typography, useTheme } from '@mui/material';

import * as OpcUa from 'opcua-webapi';
import * as Web from '../Web';
import { SessionContext } from '../SessionProvider';

import { styled } from '@mui/material/styles';
import { IMonitoredItem, SubscriptionContext } from '../SubscriptionProvider';
import { HandleFactory } from '../service/HandleFactory';
import { SubscriptionState } from '../service/SubscriptionState';
import { SessionState } from '../service/SessionState';
import { IRequestMessage } from '../service/IRequestMessage';

const CustomButton = styled(Button)(({ theme }) => ({
   paddingLeft: 0,
   paddingRight: 0,
   minWidth: 'unset',
   '& .MuiSvgIcon-root': {
      color: theme.palette.primary.dark
   }
}));

export const SessionStatusBar = () => {
   const theme = useTheme();
   const [currentTime, setCurrentTime] = React.useState<OpcUa.DataValue | undefined>();
   const timer = React.useRef<NodeJS.Timeout | null>(null);
   const [clientHandle] = React.useState<number>(HandleFactory.increment());
   const [monitoredItems] = React.useState<IMonitoredItem[]>([
      {
         itemHandle: 2,
         nodeId: OpcUa.VariableIds.Server_ServerStatus,
         path: ['State'],
      }]);

   const {
      sessionState,
      sendRequest,
      setIsEnabled,
      setIsSessionEnabled,
      lastCompletedRequest
   } = React.useContext(SessionContext);

   const {
      setIsSubscriptionEnabled,
      subscriptionState,
      lastSequenceNumber,
      subscribe,
   } = React.useContext(SubscriptionContext);

   React.useEffect(() => {
      if (lastCompletedRequest?.clientHandle === clientHandle) {
         if (lastCompletedRequest?.response?.ServiceId === OpcUa.DataTypeIds.ReadResponse) {
            const rrm = lastCompletedRequest.response.Body as OpcUa.ReadResponse;
            setCurrentTime(rrm?.Results?.at(0));
         }
      }
   }, [lastCompletedRequest, clientHandle]);

   React.useEffect(() => {
      if (subscriptionState === SubscriptionState.Open) {
         const itemsToCreate = monitoredItems.filter(item => !item.monitoredItemId && !item.creationError);
         if (itemsToCreate.length) {
            subscribe(itemsToCreate, clientHandle);
         }
      }
   }, [subscriptionState, monitoredItems, clientHandle, subscribe]);

   const sendRead = React.useCallback(() => {
      const request: OpcUa.ReadRequest= {
         NodesToRead: [
            {
               NodeId: OpcUa.VariableIds.Server_ServerStatus_CurrentTime,
               AttributeId: OpcUa.Attributes.Value
            }
         ]
      };
      const message: IRequestMessage = {
         ServiceId: OpcUa.DataTypeIds.ReadRequest,
         Body: request
      };
      sendRequest(message, clientHandle);
   }, [sendRequest, clientHandle]);

   const startTimer = React.useCallback(() => {
      if (timer.current !== null) return; // Prevent starting multiple timers
      timer.current = setInterval(() => {
         sendRead();
      }, 1000);
   }, [sendRead]);

   const stopTimer = React.useCallback(() => {
      if (timer.current !== null) {
         clearInterval(timer.current);
         timer.current = null;
      }
   }, []);

   const handleConnect = React.useCallback((state: SessionState) => {
      if (state === SessionState.Disconnected) {
         setIsSubscriptionEnabled(true);
         setIsSessionEnabled(true);
         setIsEnabled(true);
      }
      else if (state === SessionState.SessionActive || state == SessionState.Error) {
         setIsSessionEnabled(false);
      }
      else if (state === SessionState.NoSession) {
         setIsEnabled(false);
      }
   }, [setIsEnabled, setIsSessionEnabled, setIsSubscriptionEnabled]);

   React.useEffect(() => {
      if (sessionState === SessionState.SessionActive) {
         startTimer();
      }
      else {
         stopTimer();
      }
   }, [sessionState, startTimer, stopTimer]);

   return (
      <Toolbar variant='dense' disableGutters sx={{ py: 0, minHeight: '36px', justifyContent: 'space-between' }}>
         <Box ml={6} sx={{ flexGrow: 0, display: { xs: 'none', color: theme.palette.text.primary, md: 'flex' } }}>
            <CustomButton sx={{ mr: 2 }} onClick={() => handleConnect(sessionState)}>
               {(sessionState === SessionState.Disconnected) ? <PlayArrowIcon /> : <StopIcon />}
            </CustomButton>
            <Button sx={{ mr: 2 }} onClick={() => handleConnect(sessionState)} >
               <Typography variant='body2' sx={{ pr: 4 }}>Session:</Typography>
               <Typography variant='body2' fontWeight={'bolder'}>{SessionState[sessionState]}</Typography>
            </Button>
            <Button sx={{ my: 2 }}>
               <Typography variant='body2' sx={{ pr: 4 }}>Server Time:</Typography>
               <Typography variant='body2' fontWeight={'bolder'}>{Web.formatTime(currentTime?.Value) ?? '---'}</Typography>
            </Button>
            <Button sx={{ my: 2 }}>
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