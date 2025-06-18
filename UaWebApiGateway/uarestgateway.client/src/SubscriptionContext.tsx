import * as React from 'react';
import { SubscriptionState } from './service/SubscriptionState';
import { ISubscriptionContext } from './SubscriptionProvider';

export const SubscriptionContext = React.createContext<ISubscriptionContext>({
    publishingInterval: 1000,
    setPublishingInterval: () => { },
    samplingInterval: 1000,
    setSamplingInterval: () => { },
    isSubscriptionEnabled: false,
    setIsSubscriptionEnabled: () => { },
    subscriptionState: SubscriptionState.Closed,
    lastSequenceNumber: 0,
    addNewMonitoredItem: () => { },
    removeMonitoredItems: () => { },
    createSubscription: () => { },
    deleteSubscription: () => { }
});
