Feature: CreateBooking
    
    @CreateBookingStartDateBeforeToday
    Scenario: Create A Booking With Start Date Before Today
        Given the booking period starts before today
        When the booking is placed
        Then booking not placed and exception is thrown

    @CreateBookingStartDateAfterEndDate
    Scenario: Create A Booking With StartDate After EndDate
        Given the booking period is starts before it ends
        When the booking is placed
        Then booking not placed and exception is thrown



    @CreatBooking
    Scenario: Create A Booking before a occupied period
        Given the booking starts before an occupied period
        And the boooking ends before an occupied period 
        When the booking is placed
        Then it should be created successfully
    
    @CreatBooking
    Scenario: Create A Booking after an occupied period
        Given the booking starts after an occupied period
        And the booking ends after an occupied period 
        When the booking is placed
        Then it should be created successfully
            
    @CreatBooking
    Scenario: Create A Booking before and after a occupied period
        Given the booking starts before an occupied period
        And the booking ends after an occupied period 
        When the booking is placed
        Then it should not be created
                    
    @CreatBooking
    Scenario: Create A Booking where end is in occupied period
        Given the booking starts before an occupied period
        And the booking ends in an occupied period 
        When the booking is placed
        Then it should not be created
                            
    @CreatBooking
    Scenario: Create A Booking where start is in occupied period
        Given the booking starts in an occupied period
        And the booking ends after an occupied period 
        When the booking is placed
        Then it should not be created
                                    
    @CreatBooking
    Scenario: Create A Booking where start and end is in occupied period
        Given the booking starts in an occupied period
        And the booking ends after an occupied period 
        When the booking is placed
        Then it should not be created

    #   @CreatBooking
    #    Scenario Outline: Create A Booking
    #        Given the booking should start at <startBooking>
    #        And booking should end at <endBooking>
    #        When the booking is placed
    #        Then is should be created in the system, '<result>'
    #
    #        Examples:
    #          | startBooking | endBooking   | result |
    #          | '2021-11-10' | '2021-11-14' | false  |
    #          | '2021-11-11' | '2021-11-14' | true   |
    #          | '2021-11-11' | '2021-11-15' | false  |

