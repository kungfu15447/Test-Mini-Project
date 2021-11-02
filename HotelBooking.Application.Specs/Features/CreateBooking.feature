Feature: CreateBooking

    @CreatBooking
    Scenario Outline: Create A Booking
        Given the booking should start at <startBooking>
        And booking should end at <endBooking>
        When the booking is placed
        Then is should be created in the system, '<result>'

        Examples:
          | startBooking | endBooking   | result |
          | '2021-11-10' | '2021-11-14' | false  |
          | '2021-11-11' | '2021-11-14' | true   |
          | '2021-11-11' | '2021-11-15' | false  |

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