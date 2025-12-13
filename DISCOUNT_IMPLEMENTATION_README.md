# Discount Percentage Implementation - Summary

## Changes Made

### 1. Database Migration
**Files Created:**
- `Infrastructure_Layer\Migrations\20251211152511_AddDiscountFieldsToHotelRoom.cs`
- `Infrastructure_Layer\Migrations\20251211152511_AddDiscountFieldsToHotelRoom.Designer.cs`

**File Updated:**
- `Infrastructure_Layer\Migrations\AppDbContextModelSnapshot.cs`

**What it does:**
- Adds a `DiscountPercentage` column to the `HotelRooms` table (decimal(18,2), default: 0)
- Adds a `FinalDiscountedPrice` column to the `HotelRooms` table (decimal(18,2), default: 0)
- The `FinalDiscountedPrice` is automatically calculated and **stored in the database** when creating or updating hotel rooms
- This migration is ready to be applied to your database

### 2. Domain Model Updates
**File Updated:** `Domain_Layer\Models\Entity\HotelRoom.cs`
- Added `FinalDiscountedPrice` property (stored in database)
- Keeps the calculated `DiscountedPrice` property for on-the-fly calculations
- The `FinalDiscountedPrice` is automatically calculated and saved to database when rooms are created/updated

### 3. New DTO Created
**File:** `Application_Layer\DTO\RoomPricingDto.cs`

This DTO contains all pricing information including:
- Original price per night
- Discount percentage
- Discount amount
- Discounted price per night
- Number of nights
- Total original price
- Total discounted price
- Total savings

### 4. Service Layer Updates
**File Updated:** `Application_Layer\Interface\IHotelService.cs`
- Added method: `Task<RoomPricingDto> CalculateRoomPricingAsync(int hotelId, int roomType, int numberOfNights)`

**File Updated:** `Application_Layer\HotelService.cs`
- Implemented `CalculateRoomPricingAsync` method
- **Automatic calculation**: When creating or updating hotels, the `FinalDiscountedPrice` is automatically calculated and saved to the database
- Formula: `FinalDiscountedPrice = PricePerNight - (PricePerNight * DiscountPercentage / 100)`

### 5. Controller Updates
**File Updated:** `Presentation_Layer\Controllers\HotelController.cs`
- Added new endpoint: `GET /api/Hotel/{hotelId}/room-pricing`
- Query parameters: `roomType` (int), `numberOfNights` (int, default=1)

## How to Use

### 1. Apply the Migration
Run this command in the Infrastructure_Layer directory:
```bash
dotnet ef database update --startup-project ..\Presentation_Layer\Presentation_Layer.csproj
```

Or in Visual Studio Package Manager Console:
```bash
Update-Database
```

### 2. Using the Discount Feature

#### When Creating/Updating a Hotel:
The `DiscountPercentage` is already part of the `CreateHotelRoomDto` and can be set when creating or updating hotel rooms:

```json
{
  "hotelName": "Example Hotel",
  "rooms": [
    {
      "roomType": 0,
      "totalRooms": 10,
      "pricePerNight": 100.00,
      "discountPercentage": 15.0,
      "maxOccupancy": 2,
      "bedCount": 1,
      "roomSize": 25.0,
      "description": "Standard room with 15% discount"
    }
  ]
}
```

#### Calculate Room Pricing with Discount:
Call the new endpoint:
```
GET /api/Hotel/1/room-pricing?roomType=0&numberOfNights=3
```

**Response Example:**
```json
{
  "hotelRoomId": 5,
  "hotelId": 1,
  "hotelName": "Example Hotel",
  "roomType": 0,
  "pricePerNight": 100.00,
  "discountPercentage": 15.0,
  "discountAmount": 15.00,
  "discountedPrice": 85.00,
  "numberOfNights": 3,
  "totalOriginalPrice": 300.00,
  "totalDiscountedPrice": 255.00,
  "totalSavings": 45.00
}
```

## Entity Features

The `HotelRoom` entity has two properties for discount handling:

**1. FinalDiscountedPrice (Database Field)**
```csharp
public decimal FinalDiscountedPrice { get; set; }
```
- **Stored in the database**
- Automatically calculated and saved when creating/updating hotel rooms
- Persists the calculated discount for historical records

**2. DiscountedPrice (Calculated Property)**
```csharp
public decimal DiscountedPrice => DiscountPercentage > 0 
    ? PricePerNight - (PricePerNight * DiscountPercentage / 100) 
    : PricePerNight;
```
- Calculated on-the-fly when accessed
- Always reflects current discount percentage

## Key Benefits

? **Database Storage**: The final discounted price is stored in the database for reporting and analytics  
? **Automatic Calculation**: No manual calculation needed - the system handles it automatically  
? **Historical Data**: The stored `FinalDiscountedPrice` preserves pricing at the time of creation/update  
? **Flexibility**: Can still calculate prices on-the-fly using the `DiscountedPrice` property

## Next Steps

1. **Apply the migration** to your database
2. **Test the endpoint** using Swagger or Postman
3. **Update existing rooms** with discount percentages if needed
4. The discount will automatically be calculated and displayed when retrieving hotel information

## Room Type Enum Values

You can get room type values from:
- `GET /api/Hotel/room-types`
- `GET /api/Hotel/all-with-values`
